using STIGRADOR.FSM;
using STIGRADOR.MVVM;
using UnityEngine;

namespace STIGRADOR
{
    public class BootStrap : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        
        private EventManager _eventManager;
        private GlobalModel _globalModel;
        private ScopeModel _scopeModel;
        private FSM<FSMState> _baseFsm;
        private Tweener _tweener;

        private void Awake()
        {
            _eventManager = new EventManager();
            _globalModel = new GlobalModel(_eventManager);
            _scopeModel = new ScopeModel(_eventManager);
            _tweener = Tweener.Create();
            _baseFsm = new FSM<FSMState>("Global", _globalModel, _scopeModel, _eventManager);
            
            _eventManager.AutoBind<int>("a", Foo);
            _eventManager.AutoBind<int>("OnaChanged", Foo);
            _eventManager.AutoBind<int, int>("b", Bar);
        }

        private void Start()
        {
            _baseFsm.AddState(new InitState());
            _baseFsm.GoToState(typeof(InitState));

            _tweener.Bounce(5f, 1f, 2f, f =>
            {
                _transform.localScale = Vector3.one * f;
            });
        }

        private void Update()
        {
            _baseFsm.Update();
            _tweener?.DoUpdate(Time.deltaTime);
        }

        private void Foo(int a)
        {
            Debug.LogError(a);
            Debug.LogError(_scopeModel.GetInt("a"));
        }
        
        private void Bar(int a, int b)
        {
            Debug.LogError(a + " " + b);
        }
    }
}
