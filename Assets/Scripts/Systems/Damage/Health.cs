using System;
using UnityEngine;

namespace Systems.Damage
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _max;

        public event Action<DamageInfo> Damaged;
        public event Action Died;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _max;
        public bool IsDead => CurrentHealth <= 0f;

        private void Awake()
        {
            CurrentHealth = _max;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (IsDead)
            {
                return;
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - damageInfo.Amount);
            Damaged?.Invoke(damageInfo);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
