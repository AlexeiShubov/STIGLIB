using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public class FSMState : BaseState
    {
        protected IFSM _FSM;

        protected SystemModel _ModelGlobal => _FSM.ModelSystem;
        protected ScopeModel _Model => _FSM.Model;
        protected Binder _BinderSystem => _FSM.BinderSystem;
        protected Binder _Binder => _FSM.Binder;
        protected IInvoker _EventManagerSystem => _FSM.InvokerSystem;
        protected IInvoker _EventManager => _FSM.Invoker;

        public virtual void Init(IFSM FSM)
        {
            _FSM = FSM;
        }

        public override void Dispose()
        {
            _BinderSystem.Dispose();
            _Binder.Dispose();
        }
    }
}
