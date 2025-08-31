using System;
using System.Collections.Generic;
using STIGRADOR.MVVM;
using UnityEngine;

namespace STIGRADOR.FSM
{
    public class FSM<T> : BaseFSM<FSMState>, IFSM where T : FSMState
    {
        protected readonly Dictionary<Type, T> _states = new Dictionary<Type, T>();
        protected readonly GlobalModel _globalModel;
        protected readonly ScopeModel _scopeModel;
        protected readonly EventManager _eventManager;

        public GlobalModel GlobalModel => _globalModel;
        public ScopeModel ScopeModel => _scopeModel;
        public EventManager EventManager => _eventManager;

        public FSM(string name, GlobalModel globalModel, ScopeModel scopeModel, EventManager eventManager) : base(name)
        {
            _globalModel = globalModel;
            _scopeModel = scopeModel;
            _eventManager = eventManager;
        }

        public override void Update()
        {
            _currentState?.Update();
        }

        public override void GoToState(Type state)
        {
            _currentState?.Exit();
            
            if (!_states.ContainsKey(state))
            {
                Debug.LogError($"Type State <color=red>{state}</color> is not exist!");
                
                return;
            }
            
            _currentState = _states[state];
            _currentState.Enter();
        }

        public void AddState(T state)
        {
            if (_states.ContainsKey(state.Name))
            {
                Debug.LogError($"State {state} already exist!");
                
                return;
            }

            state.SetParentFSM(this);
            _states.Add(state.Name, state);
        }

        public void RemoveState(Type stateName)
        {
            if (!_states.ContainsKey(stateName))
            {
                Debug.LogError($"State {stateName} is not exist!");
                
                return;
            }
            
            _states.Remove(stateName);
        }
    }
}