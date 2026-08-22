namespace LayerZero.Gameplay.Characters.Common.Abilities
{
    public interface IChargeableAbility : IAbility
    {
        void Refill();
        void RefillTo(int amount);
    }
}
