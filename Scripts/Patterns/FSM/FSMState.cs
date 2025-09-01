using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public class FSMState : BaseState
    {
        protected IFSM _FSM;

        protected SystemModel GlobalModel => _FSM.SystemModel;
        protected ScopeModel ScopeModel => _FSM.ScopeModel;
        protected Binder SystemBinder => _FSM.SystemBinder;
        protected Binder ScopeBinder => _FSM.ScopeBinder;
        protected IInvoker SystemEventManager => _FSM.SystemInvoker;
        protected IInvoker ScopeEventManager => _FSM.ScopeInvoker;

        public virtual void Init(IFSM FSM)
        {
            _FSM = FSM;
        }

        public override void Dispose()
        {
            SystemBinder.Dispose();
            ScopeBinder.Dispose();
        }
    }
}
