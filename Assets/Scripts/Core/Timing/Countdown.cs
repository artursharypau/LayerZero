using UnityEngine;

namespace LayerZero.Core.Timing
{
    public struct Countdown
    {
        private float _endTime;

        public readonly bool IsExpired => Time.time >= _endTime;

        public void Start(float duration)
        {
            _endTime = Time.time + Mathf.Max(0f, duration);
        }

        public void Stop()
        {
            _endTime = 0f;
        }
    }
}
