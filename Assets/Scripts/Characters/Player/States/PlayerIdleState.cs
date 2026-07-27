using Characters.Common.Animation;
using Characters.Common.States;

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

            Controller.SetVelocityX(0f);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
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
