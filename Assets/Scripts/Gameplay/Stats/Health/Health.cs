using System;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Health
{
    internal sealed class Health : IDamageable, IHealth
    {
        public event Action<float> HealthChanged;
        public event Action Died;

        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;

        public void Initialize(float health)
        {
            if (MaxHealth > 0f)
            {
                return;
            }

            MaxHealth = health;
            CurrentHealth = health;
        }

        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0)
            {
                return;
            }

            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
            HealthChanged?.Invoke(CurrentHealth);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
