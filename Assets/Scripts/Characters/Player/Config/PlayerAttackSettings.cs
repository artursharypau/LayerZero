using System;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [Serializable]
    public sealed class PlayerAttackSettings
    {
        [SerializeField] private AttackComboStep[] _combo = Array.Empty<AttackComboStep>();
        [SerializeField] [Min(0f)] private float _velocityDuration = 0.1f;
        [SerializeField] [Min(0f)] private float _comboResetDelay = 1f;

        public int ComboLength => _combo?.Length ?? 0;
        public float VelocityDuration => _velocityDuration;

        /// <summary>How long after a swing the combo still continues instead of restarting.</summary>
        public float ComboResetDelay => _comboResetDelay;

        public AttackComboStep GetStep(int index)
        {
            return _combo != null && index >= 0 && index < _combo.Length ? _combo[index] : null;
        }
    }
}
