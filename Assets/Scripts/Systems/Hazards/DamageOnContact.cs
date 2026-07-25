using System.Collections.Generic;
using Systems.Damage;
using UnityEngine;

namespace Systems.Hazards
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private DamageDefinition _damage;
        [SerializeField] private float _tickInterval = 0.5f;

        private readonly Dictionary<IDamageable, float> _nextHitTime = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryDamage(other);
        }

        private void TryDamage(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
            {
                return;
            }

            if (_nextHitTime.TryGetValue(damageable, out float nextTime) && Time.time < nextTime)
            {
                return;
            }

            damageable.TakeDamage(DamageInfo.FromDefinition(_damage, transform));
            _nextHitTime[damageable] = Time.time + _tickInterval;
        }
    }
}
