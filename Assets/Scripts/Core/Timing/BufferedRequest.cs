namespace LayerZero.Core.Timing
{
    public sealed class BufferedRequest
    {
        private readonly float _lifetime;

        private Countdown _timer;
        private bool _isPending;

        public BufferedRequest(float lifetime)
        {
            _lifetime = lifetime;
        }

        public bool IsPending => _isPending && !_timer.IsExpired;

        public void Raise()
        {
            _timer.Start(_lifetime);
            _isPending = true;
        }

        public void Consume()
        {
            _timer.Stop();
            _isPending = false;
        }
    }
}
