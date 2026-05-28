using Common;

namespace Player.States
{
    public class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(StateMachine fsm, PlayerController controller)
            : base(true, PlayerAnimationIdProvider.JumpFall, fsm, controller)
        {
        }

        public override void Enter()
        {
            Jump();

            base.Enter();
        }

        public override void Update()
        {
            if (Controller.RB.linearVelocityY <= 0f)
            {
                FSM.ChangeState(Controller.FallState);
            }
            else
            {
                Jump();
            }

            base.Update();
        }

        private void Jump()
        {
            if (Controller.CanJump())
            {
                Controller.SetVelocity(Controller.MoveSpeed * Controller.MoveInput.x, Controller.JumpForce);
                Controller.ConsumeJump();
            }
        }
    }
}
