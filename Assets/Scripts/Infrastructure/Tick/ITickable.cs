namespace Infrastructure.Tick
{
    public interface ITickable
    {
        void Tick(float deltaTime);
    }
}
