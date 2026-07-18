namespace Core.Utils
{
    public class CountdownTimer
    {
        private float _time;

        public bool IsRunning => _time > 0f;

        public void Start(float duration)
        {
            _time = duration;
        }

        public bool Tick(float deltaTime)
        {
            if (!IsRunning)
            {
                return false;
            }

            _time -= deltaTime;

            return _time <= 0;
        }
    }
}
