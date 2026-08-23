namespace LayerZero.Gameplay.Characters.Common.Abilities
{
    internal interface IChargeableAbility : IAbility
    {
        void Refill();
        void RefillTo(int amount);
    }
}
