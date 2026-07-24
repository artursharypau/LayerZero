using System;
using Infrastructure.Tick;
using Infrastructure.Utils;
using UnityEngine;

namespace Characters.Player.Abilities
{
    [Serializable]
    public class DashAbility : ITickable
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] [Range(1, 5)] private float _speedMultiplier = 3f;
        [SerializeField] private float _cooldown = 2f;

        private readonly CountdownTimer _cooldownTimer;

        public DashAbility()
        {
            _cooldownTimer = new CountdownTimer();
        }

        public float Duration => _duration;
        public float SpeedMultiplier => _speedMultiplier;
        public bool IsReady => _cooldownTimer.IsExpired;

        public void Tick(float deltaTime)
        {
            _cooldownTimer.Tick(deltaTime);
        }

        public void Trigger()
        {
            _cooldownTimer.Start(_duration + _cooldown);
        }
    }
}
