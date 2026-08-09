using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyPatrolState : EnemyGroundedState
    {
        public EnemyPatrolState(EnemyController owner)
            : base(owner)
        {
            OnFixed(IsBlocked, EnemyStateId.Idle);
        }

        public override int Id => EnemyStateId.Patrol;

        public override void Enter()
        {
            base.Enter();

            if (IsBlocked())
            {
                Movement.Flip();
            }

            Animator.SetFloat(CommonAnimatorParameters.VelocityXAnimMultiplier, Config.Movement.MoveAnimationMultiplier);
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

        private bool IsBlocked()
        {
            return !Movement.IsGrounded || Movement.IsWalled;
        }
    }
}
