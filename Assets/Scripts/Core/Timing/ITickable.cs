namespace LayerZero.Core.Timing
{
    /// <summary>Anything that needs a manually driven update (pure C# objects, modules, abilities).</summary>
    public interface ITickable
    {
        void Tick(float deltaTime);
    }
}
