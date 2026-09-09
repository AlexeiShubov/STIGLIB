# MVVM Pattern — Usage Guide

Namespaces: `STIGRADOR.MVVM` (model / events / binding), `STIGRADOR.FSM` (states, integrated with MVVM).

## Architecture

Everything is built around `SystemEntity` — a container holding two parallel, non-overlapping levels:

| Level | Model | Event Manager | Typical use |
|---|---|---|---|
| **System** | `SystemModel` | `SystemEventManager` | global data/events (whole application) |
| **Scope** | `ScopeModel` | `ScopeEventManager` | local data/events (screen/feature/context) |

The library ships no ready-made bootstrap example — `SystemEntity` is assembled manually at your composition point:

```csharp
var systemEventManager = new SystemEventManager();
var scopeEventManager  = new ScopeEventManager();
var systemModel = new SystemModel(systemEventManager);
var scopeModel  = new ScopeModel(scopeEventManager);

var systemEntity = new SystemEntity(
    systemModel, scopeModel,
    new Binder(systemEventManager), new Binder(scopeEventManager),
    systemEventManager, scopeEventManager);
```

## 1. Model (`BaseModel` → `SystemModel`/`ScopeModel`)

A key-value store; `Set` automatically raises an `On{name}Changed` event.

```csharp
_Model.Set("Score", 100);                     // raises "OnScoreChanged" with value 100
_Model.Set("Score", 100, invokeEvent: false);  // no event raised

int score = _Model.Get<int>("Score", 0);      // ⚠️ hard cast, throws on type mismatch
bool en   = _Model.GetBool("Enabled", true);   // safe typed getters (try/catch inside)
int hp    = _Model.GetInt("HP");
float sp  = _Model.GetFloat("Speed");

_Model.Inc("Score");      // +1
_Model.Inc("Score", 10);  // +10
_Model.Dec("Score", 5);   // -5
```

For primitives, always prefer `GetBool`/`GetInt`/`GetFloat`/`GetLong`/`GetDouble` over `Get<T>` — they don't throw on a type mismatch.

## 2. Subscribing/invoking in plain classes (not MonoBehaviour, not FSM)

There's no automation here — you create a `Binder` wrapping the target `EventManager` yourself and dispose it when the object is destroyed.

```csharp
public class ScoreCounter : IDisposable
{
    private readonly Binder _binder;
    private readonly IInvoker _invoker;

    public ScoreCounter(ScopeEventManager scopeEventManager)
    {
        _invoker = scopeEventManager;
        _binder  = new Binder(scopeEventManager);

        _binder.Bind<int>("OnScoreChanged", OnScoreChanged); // subscribe
    }

    private void OnScoreChanged(int newScore) { /* ... */ }

    public void AddPoints(int amount)
    {
        _invoker.Invoke("OnPointsAdded", amount); // invoke (raise) an event — no Binder needed for this
    }

    public void Dispose() => _binder.Dispose(); // unsubscribes ALL bindings made through this binder at once
}
```

- `Bind`/`UnBind` are overloaded for 0–3 arguments (`Action`, `Action<A>`, `Action<A,B>`, `Action<A,B,C>`).
- You can raise an event without a `Binder` at all — just an `IInvoker` reference and `Invoke(...)`.
- Re-subscribing to the same event name with a **different delegate signature** is silently ignored (logged via `Debug.LogError` in `BindingsStorage.WriteEvent`) — keep one signature per event name.

## 3. Subscribing/invoking from FSM states

`FSMState` already carries ready-to-use `Binder`/`IInvoker` pairs for both levels — created automatically in `Init()`, which is called from `FSM.AddState(...)`:

```
_BinderSystem, _Binder                 // system/scope Binders
_EventManagerSystem, _EventManager     // system/scope IInvoker
_ModelGlobal, _Model                   // SystemModel/ScopeModel
```

```csharp
public class GameplayState : FSMState
{
    public override void Init(IFSM fsm)
    {
        base.Init(fsm); // required — without it _Binder/_BinderSystem stay null

        _Binder.Bind<int>("OnScoreChanged", OnScoreChanged);
    }

    public override void Enter()
    {
        base.Enter(); // logs "Enter state ---> ..."
        _Model.Set("IsPlaying", true); // raises "OnIsPlayingChanged"
    }

    public override void Exit()
    {
        base.Exit();
        _Model.Set("IsPlaying", false);
    }

    private void OnScoreChanged(int score)
    {
        if (score >= 100) _FSM.StartState(typeof(WinState)); // switch state
    }

    public override void Dispose() => base.Dispose(); // unsubscribes _Binder and _BinderSystem
}
```

Registration and startup:

```csharp
var fsm = new FSM("GameFSM", systemEntity);
fsm.AddState(new InitState());
fsm.AddState(new GameplayState());
fsm.AddState(new WinState());
fsm.StartState(typeof(InitState));

fsm.DoUpdate(); // call every frame, e.g. from MonoBehaviour.Update
```

Important notes:
- `Init()` is called automatically from `AddState` — don't call it manually, but always call `base.Init(fsm)` when overriding it.
- `Dispose()` on `BaseState` already unsubscribes both binders — when overriding `Dispose`, always call `base.Dispose()` too.
- ⚠️ **`FSM.RemoveState` does not call `Dispose()`** on the removed state — if you remove a state from a live FSM manually, dispose it yourself, otherwise its subscriptions stay active (a leak).
- States communicate either via `_Model.Set(...)` (event + stored value) or directly via `_EventManager.Invoke(...)` (event only, no stored value).

## 4. MonoBehaviour / UI

For `MonoBehaviour`, use `BinderMonoBehaviour`:

```csharp
public class HealthBarView : BinderMonoBehaviour
{
    public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
    {
        base.Initialize(scopeModel, scopeEventManager); // creates _Binder, fills _Model/_Invoker
        _Binder.Bind<int>("OnHpChanged", OnHpChanged);
    }

    private void OnHpChanged(int hp) { /* update UI */ }
    // OnDestroy is already overridden in the base class → _Binder.Dispose()
}
```

Unity never calls `Initialize(...)` itself (it's not `Awake`/`Start`) — someone external must call it explicitly.

⚠️ Unlike `FSMState`, `BinderMonoBehaviour` only exposes the **Scope level** — there are no `_BinderSystem`/`_EventManagerSystem` fields here.

Ready-made click components exist: `UIButtonBind` (wraps `UnityEngine.UI.Button`) and `UIClickBind` (wraps `EventTrigger.PointerDown`) — both inherit `UIBindBase`, which auto-generates event names from the `GameObject` name (`_eventName`, `On{_eventName}Click`, `On{_eventName}EnableChanged`) and toggles interactability via `_Model.GetBool(_enableField, ...)`. Usually it's enough to attach the component and set `_eventName` in the inspector.
