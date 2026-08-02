using System;
using UnityEngine;

namespace Systems.Damage
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] [Min(1)] private int _max = 1;

        public event Action<DamageInfo> Damaged;
        public event Action Died;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => _max;
        public bool IsDead => CurrentHealth <= 0;

        private void Awake()
        {
            CurrentHealth = _max;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            if (IsDead || damageInfo.Amount <= 0)
            {
                return;
            }

            CurrentHealth = Mathf.Max(0, CurrentHealth - damageInfo.Amount);
            Damaged?.Invoke(damageInfo);

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
