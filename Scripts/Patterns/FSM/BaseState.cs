using System;
using STIGRADOR.Utils;
using UnityEngine;

namespace STIGRADOR.FSM
{
    public abstract class BaseState : IUpdatable
    {
        public Type Name { get; }

        public BaseState()
        {
            Name = GetType();
        }

        public virtual void Enter()
        {
            Debug.Log($"<color=green>Enter</color> state ---> {Name}");
        }

        public virtual void Exit()
        {
            Debug.Log($"<color=red>Exit</color> state ---> {Name}");
        }

        public virtual void DoUpdate(float deltaTime) { }
    }
}
