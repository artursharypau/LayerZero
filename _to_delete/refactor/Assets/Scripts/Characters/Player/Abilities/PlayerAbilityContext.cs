using Characters.Common.Movement;
using Characters.Player.Input;

namespace Characters.Player.Abilities
{
    public class PlayerAbilityContext : IPlayerAbilityContext
    {
        public PlayerAbilityContext(IMovementState movementState, IPlayerInput input)
        {
            MovementState = movementState;
            Input = input;
        }

        public IMovementState MovementState { get; }
        public IPlayerInput Input { get; }
    }
}
