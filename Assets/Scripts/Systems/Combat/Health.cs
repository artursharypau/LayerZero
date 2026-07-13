using System;
using UnityEngine;

namespace Systems.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _max;

        private int _current;

        public event Action<DamageInfo> Damaged;

        private void Awake()
        {
            _current = _max;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            _current -= damageInfo.Amount;

            Damaged?.Invoke(damageInfo);
        }
    }
}
