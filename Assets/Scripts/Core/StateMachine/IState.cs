namespace LayerZero.Core.StateMachine
{
    public interface IState
    {
        int Id { get; }

        void Enter();
        bool TryTransition();
        bool TryFixedTransition();
        void Update();
        void FixedUpdate();
        void Exit();
    }
}
