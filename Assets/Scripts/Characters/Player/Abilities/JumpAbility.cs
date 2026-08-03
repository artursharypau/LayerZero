using LayerZero.Characters.Common.Abilities;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.Abilities
{
    public sealed class JumpAbility : IChargeableAbility
    {
        private readonly AbilityCharges _charges;
        private readonly IPlayerInput _input;

        public JumpAbility(JumpAbilitySettings settings, IPlayerInput input)
        {
            _charges = new AbilityCharges(settings.Charges);
            _input = input;
        }

        public int AvailableCharges => _charges.Available;

        public bool CanUse()
        {
            return _charges.HasCharges && _input.WasPerformed(PlayerInputAction.Jump);
        }

        public void Use()
        {
            _input.Consume(PlayerInputAction.Jump);
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
