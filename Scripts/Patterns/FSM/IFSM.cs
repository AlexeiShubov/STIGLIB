using System;
using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public interface IFSM
    {
       SystemModel ModelSystem { get; }
       ScopeModel Model { get; }
       Binder BinderSystem { get; }
       Binder Binder { get; }
       IInvoker InvokerSystem { get; }
       IInvoker Invoker { get; }

       void StartState(Type nextState);
    }
}
