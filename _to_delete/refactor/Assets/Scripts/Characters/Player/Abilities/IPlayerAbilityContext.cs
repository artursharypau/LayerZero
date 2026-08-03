using Characters.Common.Movement;
using Characters.Player.Input;

namespace Characters.Player.Abilities
{
    public interface IPlayerAbilityContext
    {
        IMovementState MovementState { get; }
        IPlayerInput Input { get; }
    }
}
