using System;
using LayerZero.Gameplay.Combat.Damage.Protections;
using LayerZero.Gameplay.Stats.Health;

namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal sealed class DamageReceiver : IDamageReceiver
    {
        private readonly IHealth _health;
        private readonly IDamageable _damageable;
        private readonly IDamageProtection _protection;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> ImpactReceived;

        public DamageReceiver(IHealth health, IDamageable damageable, IDamageProtection protection)
        {
            _health = health;
            _damageable = damageable;
            _protection = protection;
        }

        public bool IsDead => _health.IsDead;

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (_health.IsDead || _protection.IsInvulnerable)
            {
                return;
            }

            if (damageInfo.Amount > 0f)
            {
                _damageable.TakeDamage(damageInfo.Amount);
                Damaged?.Invoke(damageInfo);
            }

            if (damageInfo.Impact.HasImpact)
            {
                ImpactReceived?.Invoke(damageInfo.Impact);
            }
        }

        public void TakePeriodicDamage(float amount)
        {
            if (_health.IsDead || _protection.IsInvulnerable || amount <= 0f)
            {
                return;
            }

            _damageable.TakeDamage(amount);
        }
    }
}
