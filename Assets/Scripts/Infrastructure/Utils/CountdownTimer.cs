using Infrastructure.Tick;

namespace Infrastructure.Utils
{
    public class CountdownTimer : ITickable
    {
        private float _time;

        public bool IsExpired => _time <= 0f;

        public void Start(float duration)
        {
            _time = duration;
        }

        public void Tick(float deltaTime)
        {
            if (!IsExpired)
            {
                _time -= deltaTime;
            }
        }
    }
}
