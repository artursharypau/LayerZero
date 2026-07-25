using Characters.Player.Abilities.Config;
using Characters.Player.Input;
using Core.Utils;

namespace Characters.Player.Abilities.Tickable
{
    public class PlayerDashAbility : IPlayerTickableAbility
    {
        private readonly float _cooldown;
        private readonly CountdownTimer _cooldownTimer = new();

        public PlayerDashAbility(PlayerDashAbilityConfig config)
        {
            _cooldown = config.Duration + config.Cooldown;
        }

        public void Tick(float deltaTime)
        {
            _cooldownTimer.Tick(deltaTime);
        }

        public bool CanBeUsed(IPlayerAbilityContext context)
        {
            return _cooldownTimer.IsExpired && !context.MovementState.IsWalled && context.Input.WasPerformed(PlayerInputAction.Dash);
        }

        public void Trigger(IPlayerAbilityContext context)
        {
            context.Input.Consume(PlayerInputAction.Dash);
            _cooldownTimer.Start(_cooldown);
        }
    }
}
