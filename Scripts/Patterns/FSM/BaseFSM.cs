using System;

namespace STIGRADOR.FSM
{
    public abstract class BaseFSM<T> where T : BaseState
    {
        protected T _currentState;
        
        public string Name { get; }

        public BaseFSM(string name)
        {
            Name = name;
        }

        public abstract void Update();
        public abstract void GoToState(Type state);
    }
}
