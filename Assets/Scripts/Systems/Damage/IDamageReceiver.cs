using System;
using Systems.Damage.Resistance;

namespace Systems.Damage
{
    public interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> DamageImpactReceived;

        void SetDamageResistanceApplier(IDamageResistanceApplier resistanceApplier);
        void TakeDamage(DamageInfo damageInfo);
    }
}
