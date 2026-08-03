using System;
using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    /// <summary>
    /// One swing of a combo. Keeping the lunge velocity and the damage together removes the
    /// parallel-array coupling the combo used to have.
    /// </summary>
    [Serializable]
    public sealed class AttackComboStep
    {
        [SerializeField] private Vector2 _velocity = new(3f, 1.5f);
        [SerializeField] private AttackDefinition _attack = new();

        public Vector2 Velocity => _velocity;
        public AttackDefinition Attack => _attack;
    }
}
