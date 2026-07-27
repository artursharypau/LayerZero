using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Player.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(PlayerController controller)
            : base(controller, AnimatorHashProvider.Move)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.InputHandler.Move.x == 0f)
            {
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (IsRunningIntoWall())
            {
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Controller.SetVelocityX(Controller.MoveSpeed * Controller.InputHandler.Move.x, true);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocityX(0f);
        }
    }
}
