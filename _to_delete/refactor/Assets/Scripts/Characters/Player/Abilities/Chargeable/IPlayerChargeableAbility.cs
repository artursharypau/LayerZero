namespace Characters.Player.Abilities.Chargeable
{
    public interface IPlayerChargeableAbility : IPlayerAbility
    {
        void Refill();
        void RefillTo(int amount);
    }
}
