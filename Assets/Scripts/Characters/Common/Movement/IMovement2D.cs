namespace LayerZero.Characters.Common.Movement
{
    public interface IMovement2D : IPositioned
    {
        bool IsGrounded { get; }
        bool IsWalled { get; }
        bool IsFalling { get; }
        float VelocityX { get; }
        float VelocityY { get; }
        float GravityScale { get; }

        void SetVelocity(float x, float y, bool updateFacing = false);
        void SetVelocityX(float x, bool updateFacing = false);
        void FaceTowards(float direction);
        void Flip();
        void SetGravityScale(float scale);
    }
}
