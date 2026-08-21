using System;

namespace LayerZero.Combat.Damage
{
    public interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        bool TakeDamage(DamageInfo damageInfo);
    }
}
