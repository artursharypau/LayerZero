using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Enemies.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyChaseState : EnemyState
    {
        private float _defaultAnimationMultiplier;

        public EnemyChaseState(EnemyController owner)
            : base(owner, EnemyAnimatorParameters.Chase)
        {
            On(() => !Perception.HasTarget, EnemyStateId.Patrol);
            On(() => Owner.Combat.IsInRange(Config.Attack.Kind, Perception.Target), EnemyStateId.Attack);
        }

        public override int Id => EnemyStateId.Chase;

        public override void Enter()
        {
            base.Enter();

            _defaultAnimationMultiplier = Animator.GetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier);
            Animator.SetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier, Config.Chase.AnimationMultiplier);
        }

        public override void Update()
        {
            base.Update();

            Animator.SetFloat(CommonAnimatorParameters.VelocityX, Movement.VelocityX);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Perception.IsTargetBehind)
            {
                Movement.Flip();
            }

            float velocityX = Movement.IsGrounded && !Movement.IsWalled
                ? Config.Movement.MoveSpeed * Config.Chase.SpeedMultiplier * Perception.DirectionToTarget
                : 0f;

            Movement.SetVelocityX(velocityX);
        }

        public override void Exit()
        {
            base.Exit();

            Animator.SetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier, _defaultAnimationMultiplier);
        }
    }
}
