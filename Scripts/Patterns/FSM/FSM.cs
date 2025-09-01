using System;
using System.Collections.Generic;
using STIGRADOR.MVVM;
using UnityEngine;

namespace STIGRADOR.FSM
{
    public class FSM : BaseFSM<FSMState>, IFSM
    {
        protected readonly Dictionary<Type, FSMState> _states = new Dictionary<Type, FSMState>();

        public SystemModel SystemModel { get; }
        public ScopeModel ScopeModel { get; }
        public Binder SystemBinder { get; }
        public Binder ScopeBinder { get; }
        public IInvoker SystemInvoker { get; }
        public IInvoker ScopeInvoker { get; }

        public FSM(string name, SystemEntity systemEntity) : base(name)
        {
            SystemModel = systemEntity.SystemModel;
            ScopeModel = systemEntity.ScopeModel;
            SystemBinder = systemEntity.SystemBinder;
            ScopeBinder = systemEntity.ScopeBinder;
            SystemInvoker = systemEntity.SystemInvoker;
            ScopeInvoker = systemEntity.ScopeInvoker;
        }

        public override void DoUpdate()
        {
            _currentState?.DoUpdate(Time.deltaTime);
        }

        public void StartState(Type nextState)
        {
            GoToState(nextState);
        }

        protected override void GoToState(Type state)
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

        public void AddState(FSMState state)
        {
            if (_states.ContainsKey(state.Name))
            {
                Debug.LogError($"State {state} already exist!");
                
                return;
            }
            
            state.Init(this);
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