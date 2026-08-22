using System;
using LayerZero.Gameplay.Combat.Damage.Resistance;
using UnityEngine;
using VContainer;

namespace LayerZero.Gameplay.Combat.Damage
{
    public sealed class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private IDamageable _damageable;
        private IDamageResistances _resistances;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> ImpactReceived;

        [Inject]
        public void Construct(IDamageable damageable, IDamageResistances resistances)
        {
            _damageable = damageable;
            _resistances = resistances;
        }

        public bool TakeDamage(DamageInfo damageInfo)
        {
            if (_damageable == null || _damageable.IsDead)
            {
                return false;
            }

            if (_resistances?.IsInvulnerable == true)
            {
                return false;
            }

            DamageImpactInfo impact = _resistances?.Resolve(damageInfo.Impact) ?? damageInfo.Impact;
            DamageInfo resolved = damageInfo.WithImpact(impact);

            _damageable.TakeDamage(resolved.Amount);
            Damaged?.Invoke(resolved);

            if (impact.HasImpact && !_damageable.IsDead)
            {
                ImpactReceived?.Invoke(impact);
            }

            return true;
        }
    }
}
