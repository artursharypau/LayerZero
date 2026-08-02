using System;
using UnityEngine;

namespace Systems.Damage
{
    [Serializable]
    public class DamageResistance
    {
        [SerializeField] private bool _isInvulnerable;
        [SerializeField] private bool _canBeStunned = true;
        [SerializeField] [Min(0f)] private float _knockbackMultiplier = 1f;

        public DamageResistance()
        {
        }

        public DamageResistance(bool isInvulnerable, bool canBeStunned, float knockbackMultiplier)
        {
            _isInvulnerable = isInvulnerable;
            _canBeStunned = canBeStunned;
            _knockbackMultiplier = knockbackMultiplier;
        }

        public bool IsInvulnerable => _isInvulnerable;
        public bool CanBeStunned => _canBeStunned;
        public float KnockbackMultiplier => _knockbackMultiplier;
    }
}
