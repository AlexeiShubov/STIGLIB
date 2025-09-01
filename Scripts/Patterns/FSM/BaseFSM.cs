using System;
using STIGRADOR.Utils;

namespace STIGRADOR.FSM
{
    public abstract class BaseFSM<T> where T : BaseState, IUpdatable
    {
        protected T _currentState;
        
        public string Name { get; }

        public BaseFSM(string name)
        {
            Name = name;
        }

        public abstract void DoUpdate();
        protected abstract void GoToState(Type state);
    }
}
