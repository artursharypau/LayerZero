using Infrastructure.Animation;
using Infrastructure.StateMachine;

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

            if (Controller.InputHandler.Move.x == 0f || IsRunningIntoWall())
            {
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Controller.SetHorizontalVelocity(Controller.MoveSpeed * Controller.InputHandler.Move.x);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetHorizontalVelocity(0f);
        }
    }
}
