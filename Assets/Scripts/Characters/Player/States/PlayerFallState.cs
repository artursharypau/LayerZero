using Characters.Player.Abilities;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerFallState : PlayerInAirState
    {
        public PlayerFallState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpFall)
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
                FSM.ChangeState(Controller.JumpState);
                return true;
            }

            if (Controller.IsGrounded)
            {
                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.WallSlideState);
                return true;
            }

            return false;
        }
    }
}
