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

        [Header("Effects")]
        [SerializeField] private bool _canApplyElementalEffect = true;

        public float Multiplier => _multiplier;
        public DamageSource Source => _source;

        public Vector2 Knockback => _knockback;
        public float StunDuration => _stunDuration;

        public bool CanApplyElementalEffect => _canApplyElementalEffect;

        public bool HasImpact => _knockback != Vector2.zero || _stunDuration > 0f;
    }
}
