using Common;

namespace Player.States
{
    public class PlayerFallState : PlayerInAirState
    {
        public PlayerFallState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimationHashProvider.JumpFall)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanJump())
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
