using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    internal sealed class PlayerAttackConfig
    {
        [SerializeField] private AttackComboStep[] _comboSteps = Array.Empty<AttackComboStep>();
        [SerializeField] [Min(0f)] private float _velocityDuration = 0.1f;
        [SerializeField] [Min(0f)] private float _comboResetDelay = 1f;

        public int ComboLength => _comboSteps?.Length ?? 0;
        public float VelocityDuration => _velocityDuration;

        public float ComboResetDelay => _comboResetDelay;

        public AttackComboStep GetComboStep(int index)
        {
            return _comboSteps != null && index >= 0 && index < _comboSteps.Length ? _comboSteps[index] : null;
        }
    }
}
