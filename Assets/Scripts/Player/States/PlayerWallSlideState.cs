using Common;

namespace Player.States
{
    public class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(StateMachine fsm, PlayerController controller)
            : base(false, PlayerAnimationIdProvider.WallSlide, fsm, controller)
        {
        }

        public override void Enter()
        {
            Controller.ResetJump();
            Controller.ConsumeJump();

            base.Enter();
        }

        public override void Update()
        {
            if (Controller.IsGrounded)
            {
                if (Controller.FacingDirection != Controller.MoveInput.x)
                {
                    Controller.Flip();
                }

                FSM.ChangeState(Controller.IdleState);
            }
            else if (!Controller.IsWalled && Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
            }
            else if (Controller.InputActions.Jump.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.WallJumpState);
            }
            else
            {
                HandleSliding();
            }

            base.Update();
        }

        private void HandleSliding()
        {
            float velocityY = Controller.MoveInput.y < 0f
                ? Controller.RB.linearVelocityY
                : Controller.RB.linearVelocityY * Controller.WallSlideMultiplier;

            Controller.SetVelocity(Controller.MoveInput.x, velocityY);
        }
    }
}
