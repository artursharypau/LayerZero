namespace LayerZero.Characters.Common.Abilities
{
    public interface IChargeableAbility : IAbility
    {
        int AvailableCharges { get; }

        void Refill();
        void RefillTo(int amount);
    }
}
