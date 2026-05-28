using Common;

namespace Player.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(StateMachine fsm, PlayerController controller)
            : base(PlayerAnimationIdProvider.Move, fsm, controller)
        {
        }

        public override void Update()
        {
            if (Controller.MoveInput.x == 0f || IsRunningIntoWall())
            {
                FSM.ChangeState(Controller.IdleState);
            }
            else
            {
                Controller.SetVelocity(Controller.MoveSpeed * Controller.MoveInput.x, Controller.RB.linearVelocityY);
            }

            base.Update();
        }

        public override void Exit()
        {
            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);

            base.Exit();
        }
    }
}
