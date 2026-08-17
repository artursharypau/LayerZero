using System;
using LayerZero.Combat.Attack;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [Serializable]
    public class PlayerCounterattackConfig
    {
        [SerializeField] private float _windowWaitingDuration;
        [SerializeField] private AttackDefinition _attack = new();

        public float WindowWaitingDuration => _windowWaitingDuration;
        public AttackDefinition Attack => _attack;
    }
}
