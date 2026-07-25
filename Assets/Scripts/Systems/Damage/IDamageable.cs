using System;

namespace Systems.Damage
{
    public interface IDamageable
    {
        event Action<DamageInfo> Damaged;
        event Action Died;

        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(DamageInfo damageInfo);
    }
}
