namespace LayerZero.Core.StateMachine
{
    public enum StateTransitionMode
    {
        /// <summary>Applied at the beginning of the next machine step. Default, keeps the current step consistent.</summary>
        Deferred = 0,

        /// <summary>Applied right away. For reactions that must not wait a frame (damage, death).</summary>
        Immediate = 1
    }
}
