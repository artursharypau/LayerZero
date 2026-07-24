using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(PlayerController controller)
            : base(controller, AnimatorHashProvider.Idle)
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
                Controller.ChangeState(StateId.Move);
                return true;
            }

            return false;
        }
    }
}
