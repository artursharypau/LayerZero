using Characters.Common.States;

namespace Characters.Enemy.States
{
    public class EnemyPatrolState : EnemyGroundedState
    {
        public EnemyPatrolState(EnemyController controller)
            : base(controller, EnemyAnimatorHashProvider.Patrol)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                Controller.Flip();
            }

            Anim.SetFloat(EnemyAnimatorHashProvider.MoveAnimMultiplier, Controller.MoveAnimMultiplier);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (!Controller.IsGrounded || Controller.IsWalled)
            {
                Controller.ChangeState(StateId.Idle);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Controller.SetVelocityX(Controller.MoveSpeed * Controller.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocityX(0f);
        }
    }
}
