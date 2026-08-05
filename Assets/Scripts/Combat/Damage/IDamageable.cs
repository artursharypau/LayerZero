using System;

namespace LayerZero.Combat.Damage
{
    public interface IDamageable
    {
        event Action<int> HealthChanged;
        event Action Died;

        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(int amount);
    }
}
