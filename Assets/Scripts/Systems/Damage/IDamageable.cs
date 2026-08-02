using System;

namespace Systems.Damage
{
    public interface IDamageable
    {
        event Action Died;

        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(int amount);
    }
}
