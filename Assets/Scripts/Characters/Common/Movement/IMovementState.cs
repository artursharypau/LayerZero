namespace LayerZero.Characters.Common.Movement
{
    /// <summary>Read-only view of movement. Given to systems that observe but must not steer.</summary>
    public interface IMovementState : IPositioned
    {
        bool IsGrounded { get; }
        bool IsWalled { get; }
        bool IsFalling { get; }
        float VelocityX { get; }
        float VelocityY { get; }
        float GravityScale { get; }
    }
}
