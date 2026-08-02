using Characters.Common.States;
using Characters.Player.Abilities;

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
                Controller.ChangeState(StateId.Jump);
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
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            if (Controller.Movement.IsWalled)
            {
                Controller.ChangeState(StateId.WallSlide);
                return true;
            }

            return false;
        }
    }
}
