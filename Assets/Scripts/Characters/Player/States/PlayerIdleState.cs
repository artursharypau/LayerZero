using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, AnimatorHashProvider.Idle)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.SetHorizontalVelocity(0f);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.InputHandler.Move.x != 0f && !IsRunningIntoWall())
            {
                FSM.ChangeState(Controller.MoveState);
                return true;
            }

            return false;
        }
    }
}
