using Core.Animation;
using Core.StateMachine;

namespace Characters.Player.States
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, AnimatorHashProvider.Move)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.MoveInput.x == 0f || IsRunningIntoWall())
            {
                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Controller.SetVelocity(
                Controller.MoveSpeed * Controller.MoveInput.x,
                Controller.RB.linearVelocityY);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
        }
    }
}
