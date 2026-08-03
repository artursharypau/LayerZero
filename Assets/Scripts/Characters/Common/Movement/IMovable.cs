namespace LayerZero.Characters.Common.Movement
{
    public interface IMovable : IMovementState
    {
        void SetVelocity(float x, float y, bool updateFacing = false);
        void SetVelocityX(float x, bool updateFacing = false);
        void Stop();
        void Flip();
        void FaceTowards(float direction);
        void SetGravityScale(float scale);
    }
}
