namespace LayerZero.Core.StateMachine
{
    public interface IState
    {
        void Enter();

        bool TryTransition();

        bool TryFixedTransition();

        void Update();

        void FixedUpdate();

        void Exit();
    }
}
