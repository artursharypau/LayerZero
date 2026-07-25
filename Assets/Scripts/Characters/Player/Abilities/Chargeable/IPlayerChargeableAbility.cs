namespace Characters.Player.Abilities.Chargeable
{
    public interface IPlayerChargeableAbility : IPlayerAbility
    {
        void Refill(int amount);
    }
}
