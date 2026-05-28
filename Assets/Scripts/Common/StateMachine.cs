namespace Common
{
    public class StateMachine
    {
        public StateBase Current { get; private set; }

        public void Initialize(StateBase initialStateBase)
        {
            Current = initialStateBase;
            Current.Enter();
        }

        public void ChangeState(StateBase newStateBase)
        {
            Current.Exit();
            Current = newStateBase;
            Current.Enter();
        }

        public void Update()
        {
            Current.Update();
        }
    }
}
