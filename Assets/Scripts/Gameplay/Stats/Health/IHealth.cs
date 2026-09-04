using System;

namespace LayerZero.Gameplay.Stats.Health
{
    public interface IHealth
    {
        event Action<float> HealthChanged;
        event Action Died;

        float MaxHealth { get; }
        float CurrentHealth { get; }
        bool IsDead { get; }

        void Initialize(float health);
    }
}
