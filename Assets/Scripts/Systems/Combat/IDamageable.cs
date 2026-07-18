using System;

namespace Systems.Combat
{
    public interface IDamageable
    {
        event Action<DamageInfo> Damaged;

        float CurrentHealth { get; }
        float MaxHealth { get; }

        void TakeDamage(DamageInfo damageInfo);
    }
}
