namespace LayerZero.Characters.Common.Abilities
{
    /// <summary>An ability backed by a limited pool of uses that is refilled by gameplay events.</summary>
    public interface IChargeableAbility : IAbility
    {
        int AvailableCharges { get; }

        void Refill();
        void RefillTo(int amount);
    }
}
