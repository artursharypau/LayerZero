namespace LayerZero.Core.StateMachine
{
    /// <summary>
    /// Contract of a single state. Transition checks are separated from the body
    /// so a state never runs its logic on the frame it decided to leave.
    /// </summary>
    public interface IState
    {
        void Enter();

        /// <returns><c>true</c> if a transition was requested and <see cref="Update"/> must be skipped.</returns>
        bool TryTransition();

        /// <returns><c>true</c> if a transition was requested and <see cref="FixedUpdate"/> must be skipped.</returns>
        bool TryFixedTransition();

        void Update();

        void FixedUpdate();

        void Exit();
    }
}
