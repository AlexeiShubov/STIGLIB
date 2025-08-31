using System;
using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public interface IFSM
    {
        GlobalModel GlobalModel { get; }
        ScopeModel ScopeModel { get; }
        EventManager EventManager { get; }

        void GoToState(Type nextState);
    }
}
