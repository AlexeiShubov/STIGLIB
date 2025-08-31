using STIGRADOR.MVVM;

namespace STIGRADOR.FSM
{
    public class FSMState : BaseState
    {
        protected IFSM _fsm;

        public BaseModel GlobalModel => _fsm.GlobalModel;
        public BaseModel ScopeModel => _fsm.ScopeModel;
        public EventManager EventManager => _fsm.EventManager;

        public void SetParentFSM(IFSM baseFsm)
        {
            _fsm = baseFsm;
        }
    }
}
