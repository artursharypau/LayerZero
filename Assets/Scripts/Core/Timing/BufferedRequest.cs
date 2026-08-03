namespace LayerZero.Core.Timing
{
    /// <summary>
    /// A one-shot request that stays pending for a short window after being raised.
    /// Used for input buffering ("player pressed jump slightly before landing").
    /// </summary>
    public sealed class BufferedRequest : ITickable
    {
        private readonly float _lifetime;
        private readonly CountdownTimer _timer = new();

        public BufferedRequest(float lifetime)
        {
            _lifetime = lifetime;
        }

        public bool IsPending { get; private set; }

        public void Raise()
        {
            IsPending = true;
            _timer.Start(_lifetime);
        }

        public void Consume()
        {
            IsPending = false;
            _timer.Stop();
        }

        public void Tick(float deltaTime)
        {
            if (!IsPending)
            {
                return;
            }

            _timer.Tick(deltaTime);
            if (_timer.IsExpired)
            {
                Consume();
            }
        }
    }
}
