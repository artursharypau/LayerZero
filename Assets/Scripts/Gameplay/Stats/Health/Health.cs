using System;
using LayerZero.Gameplay.Combat.Damage.Processing;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Health
{
    internal sealed class Health : IDamageable, IHealth
    {
        public event Action<float> HealthChanged;
        public event Action Died;

        public float MaxHealth { get; }
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0f;

        public Health(IStatsSystem stats)
        {
            MaxHealth = stats.Get(StatId.MaxHealth);
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f)
            {
                return;
            }

            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, MaxHealth);
            HealthChanged?.Invoke(CurrentHealth);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
