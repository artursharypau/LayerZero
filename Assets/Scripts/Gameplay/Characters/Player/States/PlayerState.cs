using LayerZero.Gameplay.Characters.Common.States;
using LayerZero.Gameplay.Characters.Player.Config;
using LayerZero.Gameplay.Characters.Player.Input;

namespace LayerZero.Gameplay.Characters.Player.States
{
    public abstract class PlayerState : CharacterState<PlayerController>
    {
        protected PlayerState(PlayerController owner)
            : base(owner)
        {
        }

        protected IPlayerInput Input => Owner.Input;
        protected PlayerConfig Config => Owner.Config;

        protected int ResolveLocomotionState()
        {
            if (Movement.IsFalling)
            {
                return PlayerStateId.Fall;
            }

            return Input.Move.x != 0f ? PlayerStateId.Move : PlayerStateId.Idle;
        }
    }
}
