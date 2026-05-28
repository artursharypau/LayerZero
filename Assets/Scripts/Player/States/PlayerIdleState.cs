using Common;

namespace Player.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(StateMachine fsm, PlayerController controller)
            : base(PlayerAnimationIdProvider.Idle, fsm, controller)
        {
        }

        public override void Enter()
        {
            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);

            base.Enter();
        }

        public override void Update()
        {
            if (Controller.MoveInput.x != 0f && !IsRunningIntoWall())
            {
                FSM.ChangeState(Controller.MoveState);
            }

            base.Update();
        }
    }
}
