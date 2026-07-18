using System;
using UnityEngine;

namespace Systems.Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _max;

        public event Action<DamageInfo> Damaged;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => _max;

        private void Awake()
        {
            CurrentHealth = _max;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            CurrentHealth -= damageInfo.Amount;

            Damaged?.Invoke(damageInfo);
        }
    }
}
