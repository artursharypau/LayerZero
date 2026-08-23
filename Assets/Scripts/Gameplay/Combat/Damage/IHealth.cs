using System;

namespace LayerZero.Gameplay.Combat.Damage
{
    public interface IHealth
    {
        event Action<int> HealthChanged;
        event Action Died;

        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsDead { get; }
    }
}
