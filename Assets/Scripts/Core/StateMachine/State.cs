namespace Core.StateMachine
{
    public abstract class State
    {
        public virtual void Enter()
        {
        }

        public virtual bool TryTransition()
        {
            return false;
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
