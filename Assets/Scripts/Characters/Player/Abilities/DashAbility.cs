using LayerZero.Characters.Common.Abilities;
using LayerZero.Characters.Common.Movement;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Player.Abilities
{
    public sealed class DashAbility : IAbility
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
