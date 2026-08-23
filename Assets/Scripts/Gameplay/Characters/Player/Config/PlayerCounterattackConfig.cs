using System;
using LayerZero.Gameplay.Combat.Attack;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    internal sealed class PlayerCounterattackConfig
    {
        [SerializeField] [Min(0f)] private float _recoveryDuration = 0.2f;
        [SerializeField] private AttackDefinition _attack = new();

        public float RecoveryDuration => _recoveryDuration;
        public AttackDefinition Attack => _attack;
    }
}
