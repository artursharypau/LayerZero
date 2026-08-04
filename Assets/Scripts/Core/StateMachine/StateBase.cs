namespace LayerZero.Core.StateMachine
{
    public abstract class StateBase
    {
        public abstract int Id { get; }

        public virtual void Enter()
        {
        }

        public virtual bool TryTransition()
        {
            return false;
        }

        public virtual bool TryFixedTransition()
        {
            return false;
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
