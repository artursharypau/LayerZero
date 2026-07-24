using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpFall)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Jump();
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.RB.linearVelocityY <= 0f)
            {
                FSM.ChangeState(Controller.FallState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Jump();
        }

        private void Jump()
        {
            if (Controller.CanJump())
            {
                Controller.ConsumeJump();
                Controller.SetVelocity(Controller.MoveSpeed * Controller.InputHandler.Move.x, Controller.JumpAbility.Force);
            }
        }
    }
}
