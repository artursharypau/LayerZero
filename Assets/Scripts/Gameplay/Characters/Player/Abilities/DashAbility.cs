using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Common.Abilities;
using LayerZero.Gameplay.Characters.Common.Movement;
using LayerZero.Gameplay.Characters.Player.Config;
using LayerZero.Gameplay.Characters.Player.Input;

namespace LayerZero.Gameplay.Characters.Player.Abilities
{
    internal sealed class DashAbility : IAbility
    {
        private readonly DashAbilityConfig _config;
        private readonly IPlayerInput _input;
        private readonly IMovement2D _movement;

        private Countdown _cooldown;

        public DashAbility(DashAbilityConfig config, IPlayerInput input, IMovement2D movement)
        {
            _config = config;
            _input = input;
            _movement = movement;
        }

        public bool CanUse()
        {
            return _cooldown.IsExpired && !_movement.IsWalled && _input.WasPerformed(PlayerInputAction.Dash);
        }

        public void Use()
        {
            _input.Consume(PlayerInputAction.Dash);
            _cooldown.Start(_config.TotalCooldown);
        }
    }
}
