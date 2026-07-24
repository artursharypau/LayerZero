using Characters.Player.Input;

namespace Characters.Player.Abilities
{
    public interface IPlayerAbilityContext
    {
        bool IsWalled { get; }
        IPlayerInput Input { get; }
    }
}
