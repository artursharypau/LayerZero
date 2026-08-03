using UnityEngine;

namespace LayerZero.Core.Timing
{
    public sealed class CountdownTimer : ITickable
    {
        private float _duration;
        private float _remaining;

        public bool IsExpired => _remaining <= 0f;

        public void Start(float duration)
        {
            _duration = Mathf.Max(0f, duration);
            _remaining = _duration;
        }

        public void Stop()
        {
            _remaining = 0f;
        }

        public void Tick(float deltaTime)
        {
            if (_remaining > 0f)
            {
                _remaining -= deltaTime;
            }
        }
    }
}
