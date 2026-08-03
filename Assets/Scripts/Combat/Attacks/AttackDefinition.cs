using System;
using LayerZero.Combat.Damage;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    /// <summary>Authoring data for one attack: what it does and how it is delivered.</summary>
    [Serializable]
    public sealed class AttackDefinition
    {
        [SerializeField] private AttackKind _kind = AttackKind.Melee;
        [SerializeField] private DamageDefinition _damage = new();

        public AttackDefinition()
        {
        }

        public AttackDefinition(AttackKind kind, DamageDefinition damage)
        {
            _kind = kind;
            _damage = damage;
        }

        public AttackKind Kind => _kind;
        public DamageDefinition Damage => _damage;
    }
}
