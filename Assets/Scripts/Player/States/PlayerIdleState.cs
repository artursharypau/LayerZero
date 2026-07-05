using Common;
using Common.Animations;

namespace Player.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, AnimationHashProvider.Idle)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.MoveInput.x != 0f && !IsRunningIntoWall())
            {
                FSM.ChangeState(Controller.MoveState);
                return true;
            }

            return false;
        }
    }
}
