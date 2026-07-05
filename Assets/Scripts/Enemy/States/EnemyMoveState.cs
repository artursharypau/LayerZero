using Common;
using Common.Animations;

namespace Enemy.States
{
    public class EnemyMoveState : EnemyGroundedState
    {
        public EnemyMoveState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, AnimationHashProvider.Move)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                Controller.Flip();
            }

            Anim.SetFloat(EnemyAnimationIdProvider.MoveAnimMultiplier, Controller.MoveAnimMultiplier);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Controller.SetVelocity(Controller.MoveSpeed * Controller.FacingDirection, Controller.RB.linearVelocityY);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
        }
    }
}
