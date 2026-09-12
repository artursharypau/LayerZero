using System;
using LayerZero.Gameplay.Combat.Damage;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Attack
{
    [Serializable]
    internal sealed class AttackDefinition
    {
        [SerializeField] private AttackKind _kind = AttackKind.Melee;
        [SerializeField] private DamageDefinition _damage = new();

        public AttackKind Kind => _kind;
        public DamageDefinition Damage => _damage;
    }
}
