namespace Characters.Common.Movement
{
    public interface IMovable : IMovementState
    {
        void SetVelocity(float x, float y, bool updateFacing = false);
        void SetVelocityX(float x, bool updateFacing = false);
        void Flip();
        void SetGravityScale(float scale);
    }
}
