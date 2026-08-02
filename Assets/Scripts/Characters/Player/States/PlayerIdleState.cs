using Characters.Common.Animation;

namespace Characters.Player.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public override int Id => (int)PlayerStateId.Idle;

        public PlayerIdleState(PlayerController controller)
            : base(controller, AnimatorHashProvider.Idle)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.Movement.SetVelocityX(0f);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.Input.Move.x != 0f && !IsRunningIntoWall())
            {
                Controller.ChangeState(PlayerStateId.Move);
                return true;
            }

            return false;
        }
    }
}
