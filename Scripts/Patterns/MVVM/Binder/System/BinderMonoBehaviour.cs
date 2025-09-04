using UnityEngine;

namespace STIGRADOR.MVVM
{
    public abstract class BinderMonoBehaviour : MonoBehaviour
    {
        protected Binder _Binder { get; private set; }
        protected ScopeModel _Model { get; private set; }
        protected IInvoker _Invoker { get; private set; }

        public virtual void Construct(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
        {
            _Model = scopeModel;
            _Invoker = scopeEventManager;
            _Binder = new Binder(scopeEventManager);
        }

        protected virtual void OnDestroy()
        {
            _Binder.Dispose();
        }
    }
}
