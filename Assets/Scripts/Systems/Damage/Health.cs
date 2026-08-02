using System;
using UnityEngine;

namespace Systems.Damage
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] [Min(1)] private int _max = 100;

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

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
