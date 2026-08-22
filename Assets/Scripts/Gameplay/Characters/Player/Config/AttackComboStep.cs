using System;
using LayerZero.Gameplay.Combat.Attack;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    public sealed class AttackComboStep
    {
        [SerializeField] private Vector2 _velocity = new(3f, 1.5f);
        [SerializeField] private AttackDefinition _attack = new();

        public Vector2 Velocity => _velocity;
        public AttackDefinition Attack => _attack;
    }
}
