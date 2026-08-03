using System;
using UnityEngine;

namespace LayerZero.Combat.Damage
{
    /// <summary>Authoring data for a single hit. Lives in configs, never mutated at runtime.</summary>
    [Serializable]
    public sealed class DamageDefinition
    {
        [SerializeField] [Min(0)] private int _amount = 10;
        [SerializeField] private DamageSource _source = DamageSource.None;

        [Header("Impact")]
        [SerializeField] private Vector2 _knockback;
        [SerializeField] [Min(0f)] private float _stunDuration;

        public DamageDefinition()
        {
        }

        public DamageDefinition(int amount, DamageSource source, Vector2 knockback = default, float stunDuration = 0f)
        {
            _amount = amount;
            _source = source;
            _knockback = knockback;
            _stunDuration = stunDuration;
        }

        public int Amount => _amount;
        public DamageSource Source => _source;

        /// <summary>Knockback in the attacker's local space; converted to world space on hit.</summary>
        public Vector2 Knockback => _knockback;

        public float StunDuration => _stunDuration;

        public bool HasImpact => _knockback != Vector2.zero || _stunDuration > 0f;
    }
}
