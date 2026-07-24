using Infrastructure.Tick;

namespace Infrastructure.Utils
{
    public class BufferedButton : ITickable
    {
        private readonly float _duration;
        private readonly CountdownTimer _timer;

        public BufferedButton(float duration)
        {
            _duration = duration;
            _timer = new CountdownTimer();
        }

        public bool IsRequested { get; private set; }

        public void Tick(float deltaTime)
        {
            _timer.Tick(deltaTime);
            if (_timer.IsExpired)
            {
                Consume();
            }
        }

        public void Press()
        {
            IsRequested = true;
            _timer.Start(_duration);
        }

        public void Consume()
        {
            IsRequested = false;
        }
    }
}
