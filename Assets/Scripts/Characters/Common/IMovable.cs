namespace Characters.Common
{
    public interface IMovable : IMovementState
    {
        void SetVelocity(float x, float y);
        void SetHorizontalVelocity(float x);
        void Flip();
        void SetGravityScale(float scale);
    }
}
