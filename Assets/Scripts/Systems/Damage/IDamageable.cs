using System;

namespace Systems.Damage
{
    public interface IDamageable
    {
        event Action<DamageInfo> Damaged;
        event Action Died;

        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(DamageInfo damageInfo);
    }
}
