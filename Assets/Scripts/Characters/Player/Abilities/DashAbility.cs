using LayerZero.Characters.Common.Abilities;
using LayerZero.Characters.Common.Movement;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Player.Abilities
{
    public sealed class DashAbility : ITickableAbility
    {
        private readonly DashAbilitySettings _settings;
        private readonly IPlayerInput _input;
        private readonly IMovementState _movement;
        private readonly CountdownTimer _cooldown = new();

        public DashAbility(DashAbilitySettings settings, IPlayerInput input, IMovementState movement)
        {
            _settings = settings;
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
            _cooldown.Start(_settings.TotalCooldown);
        }

        public void Tick(float deltaTime)
        {
            _cooldown.Tick(deltaTime);
        }
    }
}
