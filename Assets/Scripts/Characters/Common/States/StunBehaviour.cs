using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Common.States
{
    public sealed class StunBehaviour
    {
        private const float MinDuration = 0.2f;

        private Countdown _timer;

        public bool IsFinished => _timer.IsExpired;

        public void Begin(float duration)
        {
            float resolvedDuration = Mathf.Max(MinDuration, duration);
            _timer.Start(resolvedDuration);
        }
    }
}
