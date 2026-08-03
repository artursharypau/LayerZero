using Characters.Player.Abilities.Config;
using Characters.Player.Input;

namespace Characters.Player.Abilities.Chargeable
{
    public class PlayerJumpAbility : IPlayerChargeableAbility
    {
        private readonly PlayerCharges _charges;

        public PlayerJumpAbility(PlayerJumpAbilityConfig config)
        {
            _charges = new PlayerCharges(config.Charges);
        }

        public bool CanBeUsed(IPlayerAbilityContext context)
        {
            return _charges.HasCharges && context.Input.WasPerformed(PlayerInputAction.Jump);
        }

        public void Trigger(IPlayerAbilityContext context)
        {
            context.Input.Consume(PlayerInputAction.Jump);
            _charges.Consume();
        }

        public void Refill()
        {
            _charges.Refill();
        }

        public void RefillTo(int amount)
        {
            _charges.RefillTo(amount);
        }
    }
}
