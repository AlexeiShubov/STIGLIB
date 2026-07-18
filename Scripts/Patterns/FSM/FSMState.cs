using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public class FSMState : BaseState
    {
        protected IFSM _FSM;

        private Binder _binderSystem;
        private Binder _binder;

        protected SystemModel _ModelGlobal => _FSM.ModelSystem;
        protected ScopeModel _Model => _FSM.Model;
        protected Binder _BinderSystem => _binderSystem;
        protected Binder _Binder => _binder;
        protected IInvoker _EventManagerSystem => _FSM.InvokerSystem;
        protected IInvoker _EventManager => _FSM.Invoker;

        public virtual void Init(IFSM FSM)
        {
            _FSM = FSM;
            _binderSystem = FSM.CreateSystemBinder();
            _binder = FSM.CreateScopeBinder();
        }

        public override void Dispose()
        {
            _binderSystem?.Dispose();
            _binder?.Dispose();
        }
    }
}
