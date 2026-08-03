using System;
using UnityEngine;

namespace LayerZero.Combat.Damage
{
    /// <summary>Plain health pool. Deliberately unaware of resistances, sources and reactions.</summary>
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] [Min(1)] private int _max = 100;

        public event Action Died;
        public event Action<int> HealthChanged;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => _max;
        public bool IsDead => CurrentHealth <= 0;

        private void Awake()
        {
            CurrentHealth = _max;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead || amount <= 0)
            {
                return;
            }

            SetHealth(CurrentHealth - amount);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0)
            {
                return;
            }

            SetHealth(CurrentHealth + amount);
        }

        private void SetHealth(int value)
        {
            CurrentHealth = Mathf.Clamp(value, 0, _max);
            HealthChanged?.Invoke(CurrentHealth);
        }
    }
}
