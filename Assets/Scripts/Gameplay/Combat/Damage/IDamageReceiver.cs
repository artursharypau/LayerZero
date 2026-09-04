using System;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        bool IsDead { get; }

        bool TakeDamage(DamageInfo damageInfo);
    }
}
