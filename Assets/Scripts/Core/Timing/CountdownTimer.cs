using UnityEngine;

namespace LayerZero.Core.Timing
{
    /// <summary>Allocation-free countdown driven by an explicit <see cref="Tick"/>.</summary>
    public sealed class CountdownTimer : ITickable
    {
        private float _duration;
        private float _remaining;

        public bool IsExpired => _remaining <= 0f;
        public bool IsRunning => _remaining > 0f;
        public float Remaining => Mathf.Max(0f, _remaining);

        /// <summary>0 right after <see cref="Start"/>, 1 once expired.</summary>
        public float NormalizedProgress => _duration <= 0f ? 1f : 1f - Mathf.Clamp01(_remaining / _duration);

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
