namespace Characters.Player.Abilities
{
    public interface IPlayerAbility
    {
        bool CanBeUsed(IPlayerAbilityContext context);
        void Trigger(IPlayerAbilityContext context);
    }
}
