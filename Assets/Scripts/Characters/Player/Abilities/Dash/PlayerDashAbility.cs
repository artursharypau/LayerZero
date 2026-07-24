using System;
using Characters.Player.Input;
using Infrastructure.Utils;
using UnityEngine;

namespace Characters.Player.Abilities.Dash
{
    [Serializable]
    public class PlayerDashAbility : IPlayerTickableAbility
    {
        [SerializeField] private PlayerDashAbilityConfig _config;

        private readonly CountdownTimer _cooldownTimer;

        public PlayerDashAbility()
        {
            _cooldownTimer = new CountdownTimer();
        }

        public void Tick(float deltaTime)
        {
            _cooldownTimer.Tick(deltaTime);
        }

        public bool CanBeUsed(IPlayerAbilityContext context)
        {
            return _cooldownTimer.IsExpired && !context.IsWalled && context.Input.WasPerformed(PlayerInputAction.Dash);
        }

        public void Trigger(IPlayerAbilityContext context)
        {
            _cooldownTimer.Start(_config.Duration + _config.Cooldown);
        }

        public IPlayerAbilityConfig GetConfig()
        {
            return _config;
        }
    }
}
