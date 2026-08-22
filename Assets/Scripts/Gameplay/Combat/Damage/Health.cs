using System;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    public sealed class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] [Min(1)] private int _max = 100;

        public event Action<int> HealthChanged;
        public event Action Died;

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

            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, _max);
            HealthChanged?.Invoke(CurrentHealth);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
