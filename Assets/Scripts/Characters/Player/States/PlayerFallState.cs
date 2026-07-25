using Characters.Player.Abilities;
using Core.StateMachine;

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

            if (Controller.IsGrounded)
            {
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            if (Controller.IsWalled)
            {
                Controller.ChangeState(StateId.WallSlide);
                return true;
            }

            return false;
        }
    }
}
