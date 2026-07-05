using Common;
using Common.Animations;

namespace Enemy.States
{
    public class EnemyMoveState : EnemyGroundedState
    {
        public EnemyMoveState(StateMachine fsm, EnemyController controller)
            : base(AnimationIdProvider.Move, fsm, controller)
        {
        }

        public override void Enter()
        {
            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                Controller.Flip();
            }

            Anim.SetFloat(EnemyAnimationIdProvider.MoveAnimMultiplier, Controller.MoveAnimMultiplier);

            base.Enter();
        }

        public override void Update()
        {
            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                FSM.ChangeState(Controller.IdleState);
            }
            else
            {
                Controller.SetVelocity(Controller.MoveSpeed * Controller.FacingDirection, Controller.RB.linearVelocityY);
            }

            base.Update();
        }

        public override void Exit()
        {
            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);

            base.Exit();
        }
    }
}
