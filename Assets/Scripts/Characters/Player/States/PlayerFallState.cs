using Characters.Player.Abilities;
using Characters.Player.Animation;

namespace Characters.Player.States
{
    public class PlayerFallState : PlayerInAirState
    {
        public PlayerFallState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.ChangeState(PlayerStateId.Jump);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Controller.Movement.IsGrounded)
            {
                Controller.ChangeState(PlayerStateId.Idle);
                return true;
            }

            if (Controller.Movement.IsWalled)
            {
                Controller.ChangeState(PlayerStateId.WallSlide);
                return true;
            }

            return false;
        }
    }
}
