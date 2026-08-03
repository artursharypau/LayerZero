using System;
using LayerZero.Combat.Damage.Resistance;

namespace LayerZero.Combat.Damage
{
    /// <summary>
    /// Entry point for incoming damage: applies resistances, forwards the amount to
    /// <see cref="IDamageable" /> and publishes the resolved result.
    /// </summary>
    public interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        void SetResistances(IDamageResistances resistances);
        void TakeDamage(DamageInfo damageInfo);
    }
}
