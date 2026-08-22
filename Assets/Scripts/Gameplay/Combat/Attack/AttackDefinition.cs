using System;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Attack
{
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
