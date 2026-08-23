namespace LayerZero.Gameplay.Characters.Common.Abilities
{
    internal interface IAbility
    {
        bool CanUse();
        void Use();
    }
}
