using System;
using LayerZero.Combat.Attack;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [Serializable]
    public sealed class PlayerJumpAttackConfig
    {
        [SerializeField] private Vector2 _velocity = new(3f, -5f);
        [SerializeField] private AttackDefinition _attack = new();

        public Vector2 Velocity => _velocity;
        public AttackDefinition Attack => _attack;
    }
}
