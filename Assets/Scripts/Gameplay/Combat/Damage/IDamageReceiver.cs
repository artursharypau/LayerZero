using System;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        bool TakeDamage(DamageInfo damageInfo);
    }
}
