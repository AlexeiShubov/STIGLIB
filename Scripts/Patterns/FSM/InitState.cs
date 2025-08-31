namespace STIGRADOR.FSM
{
    public class InitState : FSMState
    {
        public override void Enter()
        {
            base.Enter();
            
            EventManager.Invoke("a", 1);
            EventManager.Invoke("b", 2, 3);
            
            ScopeModel.Set("a", 4);
        }
    }
}