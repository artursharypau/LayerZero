using Characters.Common;
using Characters.Player.Input;

namespace Characters.Player.Abilities
{
    public class PlayerAbilityContext : IPlayerAbilityContext
    {
        private readonly IMovable _movable;

        public PlayerAbilityContext(IMovable movable, IPlayerInput input)
        {
            _movable = movable;
            Input = input;
        }

        public bool IsWalled => _movable.IsWalled;
        public IPlayerInput Input { get; }
    }
}
