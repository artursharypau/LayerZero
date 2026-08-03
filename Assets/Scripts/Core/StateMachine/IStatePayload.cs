namespace LayerZero.Core.StateMachine
{
    /// <summary>
    /// Implemented by states that need data handed over at transition time
    /// (e.g. a hurt state receiving the impact that caused it).
    /// </summary>
    public interface IStatePayload<in TPayload>
    {
        void SetPayload(TPayload payload);
    }
}
