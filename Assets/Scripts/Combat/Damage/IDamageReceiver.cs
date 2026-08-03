using System;
using LayerZero.Combat.Damage.Resistance;

namespace LayerZero.Combat.Damage
{
    public interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        void SetResistances(IDamageResistances resistances);
        void TakeDamage(DamageInfo damageInfo);
    }
}
