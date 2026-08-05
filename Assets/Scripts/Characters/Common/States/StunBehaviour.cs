using LayerZero.Characters.Common.Movement;
using LayerZero.Combat.Damage;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Common.States
{
    public sealed class StunBehaviour
    {
        private const float MinKnockbackLockDuration = 0.1f;

        private Countdown _timer;

        public bool IsFinished => _timer.IsExpired;

        public void Begin(IMovement2D movement, DamageImpactInfo impact)
        {
            movement.SetVelocity(impact.Knockback.x, impact.Knockback.y);

            float duration = Mathf.Max(MinKnockbackLockDuration, impact.StunDuration);
            _timer.Start(duration);
        }
    }
}
