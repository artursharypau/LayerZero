using System;

namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal interface IDamageReceiver
    {
        event Action<DamageInfo> Damaged;
        event Action<DamageImpactInfo> ImpactReceived;

        bool IsDead { get; }

        void TakeDamage(DamageInfo damageInfo);
        void TakePeriodicDamage(float amount);
    }
}
