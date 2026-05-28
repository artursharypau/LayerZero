using Common;

namespace Player.States
{
    public class PlayerFallState : PlayerInAirState
    {
        public PlayerFallState(StateMachine fsm, PlayerController controller)
            : base(true, PlayerAnimationIdProvider.JumpFall, fsm, controller)
        {
        }

        public override void Update()
        {
            if (Controller.CanJump())
            {
                FSM.ChangeState(Controller.JumpState);
            }
            else if (Controller.IsGrounded)
            {
                FSM.ChangeState(Controller.IdleState);
            }
            else if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.WallSlideState);
            }

            base.Update();
        }
    }
}
