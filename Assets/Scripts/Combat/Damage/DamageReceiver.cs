using System;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Extensions;
using UnityEngine;

namespace LayerZero.Combat.Damage
{
    /// <summary>
    /// The only component attackers talk to. Resolves resistances, forwards the amount to the
    /// health pool and publishes the result so reactions (states, VFX, UI) can subscribe.
    /// </summary>
    public sealed class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private IDamageable _damageable;
        private IDamageResistances _resistances;

        public event Action<DamageInfo> Damaged;
        public event Action<DamageImpactInfo> ImpactReceived;

        private void Awake()
        {
            _damageable = this.GetRequired<IDamageable>();
        }

        public void SetResistances(IDamageResistances resistances)
        {
            _resistances = resistances;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (_damageable == null || _damageable.IsDead)
            {
                return;
            }

            if (_resistances is { IsInvulnerable: true })
            {
                return;
            }

            DamageImpactInfo impact = _resistances?.Filter(damageInfo.Impact) ?? damageInfo.Impact;
            DamageInfo resolved = damageInfo.WithImpact(impact);

            _damageable.TakeDamage(resolved.Amount);
            Damaged?.Invoke(resolved);

            // A dead character must not be pushed into a hurt reaction - death wins.
            if (impact.HasImpact && !_damageable.IsDead)
            {
                ImpactReceived?.Invoke(impact);
            }
        }
    }
}
