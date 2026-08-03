using LayerZero.Characters.Enemies.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyPatrolState : EnemyGroundedState
    {
        public EnemyPatrolState(EnemyController owner)
            : base(owner, EnemyAnimatorParameters.Patrol)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (!Movement.IsGrounded || Movement.IsWalled)
            {
                Movement.Flip();
            }

            Animation.SetFloat(EnemyAnimatorParameters.MoveAnimationMultiplier, Config.Movement.MoveAnimationMultiplier);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (!Movement.IsGrounded || Movement.IsWalled)
            {
                ChangeTo<EnemyIdleState>();
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Movement.SetVelocityX(Config.Movement.MoveSpeed * Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            Movement.SetVelocityX(0f);
        }
    }
}
