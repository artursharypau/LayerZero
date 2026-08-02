using Characters.Common.Animation;

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

            if (Controller.Input.Move.x == 0f)
            {
                Controller.ChangeState(PlayerStateId.Idle);
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
                Controller.ChangeState(PlayerStateId.Idle);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Controller.Movement.SetVelocityX(Controller.MoveSpeed * Controller.Input.Move.x, true);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.Movement.SetVelocityX(0f);
        }
    }
}
