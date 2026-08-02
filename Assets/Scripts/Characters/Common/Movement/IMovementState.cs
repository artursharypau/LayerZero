namespace Characters.Common.Movement
{
    public interface IMovementState
    {
        bool IsGrounded { get; }
        bool IsWalled { get; }
        bool IsFalling { get; }
        float VelocityX { get; }
        float VelocityY { get; }
        float GravityScale { get; }
    }
}
