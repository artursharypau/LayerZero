using System;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    [Serializable]
    internal sealed class DamageDefinition
    {
        [SerializeField] [Min(0f)] private float _multiplier = 1f;
        [SerializeField] private DamageSource _source = DamageSource.None;

        [Header("Impact")]
        [SerializeField] private Vector2 _knockback;
        [SerializeField] [Min(0f)] private float _stunDuration;

        public DamageDefinition()
        {
        }

        public DamageDefinition(float multiplier, DamageSource source, Vector2 knockback = default, float stunDuration = 0f)
        {
            _multiplier = multiplier;
            _source = source;
            _knockback = knockback;
            _stunDuration = stunDuration;
        }

        public float Multiplier => _multiplier;
        public DamageSource Source => _source;

        public Vector2 Knockback => _knockback;
        public float StunDuration => _stunDuration;

        public bool HasImpact => _knockback != Vector2.zero || _stunDuration > 0f;
    }
}
