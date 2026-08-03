namespace LayerZero.Characters.Common.Movement
{
    /// <summary>Write access to movement. Held by states, which are the only things allowed to steer.</summary>
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
