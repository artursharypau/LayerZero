using System;
using UnityEngine;

namespace LayerZero.Characters.Player.Config
{
    [Serializable]
    public sealed class DashAbilitySettings
    {
        [SerializeField] [Min(0f)] private float _duration = 0.2f;
        [SerializeField] [Min(0f)] private float _cooldown = 2f;
        [SerializeField] [Range(1f, 5f)] private float _speedMultiplier = 3f;

        public float Duration => _duration;
        public float Cooldown => _cooldown;
        public float SpeedMultiplier => _speedMultiplier;

        /// <summary>Cooldown is counted from the start of the dash, not from its end.</summary>
        public float TotalCooldown => _duration + _cooldown;
    }
}
