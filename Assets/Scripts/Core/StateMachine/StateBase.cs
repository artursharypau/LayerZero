namespace LayerZero.Core.StateMachine
{
    /// <summary>
    /// Base class for every state. States are registered and resolved by their
    /// <see cref="System.Type" />, so a derived state transparently replaces its base
    /// (see <see cref="StateRegistry" />) - that is the main extension point for new characters.
    /// </summary>
    public abstract class StateBase : IState
    {
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
