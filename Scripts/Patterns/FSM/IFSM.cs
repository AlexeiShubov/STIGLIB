using System;
using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public interface IFSM
    {
       SystemModel SystemModel { get; }
       ScopeModel ScopeModel { get; }
       Binder SystemBinder { get; }
       Binder ScopeBinder { get; }
       IInvoker SystemInvoker { get; }
       IInvoker ScopeInvoker { get; }

       void StartState(Type nextState);
    }
}
