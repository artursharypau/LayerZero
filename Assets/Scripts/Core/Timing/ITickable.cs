namespace LayerZero.Core.Timing
{
    public interface ITickable
    {
        void Tick(float deltaTime);
    }
}
