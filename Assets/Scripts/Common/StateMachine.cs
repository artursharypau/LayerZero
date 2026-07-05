namespace Common
{
    public class StateMachine
    {
        public State Current { get; private set; }

        public void Initialize(State initialState)
        {
            Current = initialState;
            Current.Enter();
        }

        public void ChangeState(State newState)
        {
            Current.Exit();
            Current = newState;
            Current.Enter();
        }

        public void Update()
        {
            if (Current == null)
            {
                return;
            }

            if (!Current.TryTransition())
            {
                Current.Update();
            }
        }
    }
}
