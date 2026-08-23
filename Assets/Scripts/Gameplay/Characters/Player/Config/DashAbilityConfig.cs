using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.Config
{
    [Serializable]
    internal sealed class DashAbilityConfig
    {
        [SerializeField] [Min(0f)] private float _duration = 0.2f;
        [SerializeField] [Min(0f)] private float _cooldown = 2f;
        [SerializeField] [Range(1f, 5f)] private float _speedMultiplier = 3f;

        public float Duration => _duration;
        public float Cooldown => _cooldown;
        public float SpeedMultiplier => _speedMultiplier;

        public float TotalCooldown => _duration + _cooldown;
    }
}
