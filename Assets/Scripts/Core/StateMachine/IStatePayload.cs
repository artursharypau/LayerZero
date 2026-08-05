namespace LayerZero.Core.StateMachine
{
    public interface IStatePayload<in TPayload>
    {
        void SetPayload(TPayload payload);
    }
}
