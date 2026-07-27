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

        public override bool TryTransition()
        {
            if (base.TryTransition())
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

        public override void Update()
        {
            base.Update();

            Controller.SetVelocityX(Controller.MoveSpeed * Controller.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocityX(0f);
        }
    }
}
