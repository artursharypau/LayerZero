namespace Characters.Common
{
    public interface IMovementState
    {
        bool IsGrounded { get; }
        bool IsWalled { get; }
        bool IsFalling { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public float GravityScale { get; }
    }
}
