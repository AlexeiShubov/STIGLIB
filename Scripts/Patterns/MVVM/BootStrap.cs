using System;
using STIGRADOR.FSM;
using STIGRADOR.MVVM;
using UnityEngine;

namespace STIGRADOR
{
    public class BootStrap : MonoBehaviour
    {
        private EventManager _eventManager;
        private GlobalModel _globalModel;
        private ScopeModel _scopeModel;
        private FSM<FSMState> _baseFsm;

        private void Awake()
        {
            _eventManager = new EventManager();
            _globalModel = new GlobalModel(_eventManager);
            _scopeModel = new ScopeModel(_eventManager);
            _baseFsm = new FSM<FSMState>("Global", _globalModel, _scopeModel, _eventManager);
            
            _eventManager.AutoBind<int>("a", Foo);
            _eventManager.AutoBind<int>("OnaChanged", Foo);
            _eventManager.AutoBind<int, int>("b", Bar);
        }

        private void Start()
        {
            _baseFsm.AddState(new InitState());
            _baseFsm.GoToState(typeof(InitState));
        }

        private void Update()
        {
            _baseFsm.Update();
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
