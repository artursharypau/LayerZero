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

            if (!Controller.Movement.IsGrounded || Controller.Movement.IsWalled)
            {
                Controller.Movement.Flip();
            }

            Anim.SetFloat(EnemyAnimatorHashProvider.MoveAnimMultiplier, Controller.MoveAnimMultiplier);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (!Controller.Movement.IsGrounded || Controller.Movement.IsWalled)
            {
                Controller.ChangeState(EnemyStateId.Idle);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Controller.Movement.SetVelocityX(Controller.MoveSpeed * Controller.Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.Movement.SetVelocityX(0f);
        }
    }
}
