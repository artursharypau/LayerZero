using System;

namespace LayerZero.Combat.Damage
{
    public interface IDamageable
    {
        event Action Died;
        event Action<int> HealthChanged;

        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }

        void TakeDamage(int amount);
    }
}
