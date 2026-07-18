using System;
using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public interface IFSM
    {
       SystemModel ModelSystem { get; }
       ScopeModel Model { get; }
       IInvoker InvokerSystem { get; }
       IInvoker Invoker { get; }

       Binder CreateSystemBinder();
       Binder CreateScopeBinder();

       void StartState(Type nextState);
    }
}
