using System;
using LayerZero.Core.Randomness;
using LayerZero.Gameplay.Combat.Damage.Protections;
using LayerZero.Gameplay.Stats;
using LayerZero.Gameplay.Stats.Health;
using UnityEngine;
using VContainer;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal sealed class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private IHealth _health;
        private IDamageable _damageable;
        private IDamageProtection _protection;
        private IStatsSystem _statsSystem;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> ImpactReceived;

        [Inject]
        public void Construct(IHealth health, IDamageable damageable, IDamageProtection protection, IStatsSystem statsSystem)
        {
            _health = health;
            _damageable = damageable;
            _protection = protection;
            _statsSystem = statsSystem;
        }

        public bool IsDead => _health.IsDead;

        public bool TakeDamage(DamageInfo damageInfo)
        {
            if (_health.IsDead || _protection.IsInvulnerable)
            {
                return false;
            }

            float evasionChance = _statsSystem.Get(StatId.EvasionChance);
            if (Chance.Roll(evasionChance))
            {
                return false;
            }

            DamageImpactInfo impact = _protection.Resolve(damageInfo.Impact);
            DamageInfo resolved = damageInfo.WithImpact(impact);

            _damageable.TakeDamage(resolved.Amount);
            Damaged?.Invoke(resolved);

            if (impact.HasImpact)
            {
                ImpactReceived?.Invoke(impact);
            }

            return true;
        }
    }
}
