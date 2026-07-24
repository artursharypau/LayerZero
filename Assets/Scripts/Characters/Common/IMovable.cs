namespace Characters.Common
{
    public interface IMovable
    {
        bool IsGrounded { get; }
        bool IsWalled { get; }
        bool IsFalling { get; }

        void SetVelocity(float x, float y);
        void SetHorizontalVelocity(float x);
        void Flip();
    }
}
