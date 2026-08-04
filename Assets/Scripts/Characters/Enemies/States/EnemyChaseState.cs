using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Enemies.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public class EnemyChaseState : EnemyState
    {
        private float _defaultAnimationMultiplier;

        public EnemyChaseState(EnemyController owner)
            : base(owner, EnemyAnimatorParameters.Chase)
        {
        }

        public override int Id => EnemyStateId.Chase;

        public override void Enter()
        {
            base.Enter();

            _defaultAnimationMultiplier = Animation.GetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier);
            Animation.SetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier, Config.Chase.AnimationMultiplier);

            Perception.TargetLost += OnTargetLost;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (CanEngage())
            {
                ChangeTo(EnemyStateId.Attack);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Animation.SetFloat(CommonAnimatorParameters.VelocityX, Movement.VelocityX);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Perception.IsTargetBehind)
            {
                Movement.Flip();
            }

            if (!Movement.IsGrounded || Movement.IsWalled)
            {
                Movement.SetVelocityX(0f);
                return;
            }

            Movement.SetVelocityX(Config.Movement.MoveSpeed * Config.Chase.SpeedMultiplier * Perception.DirectionToTarget);
        }

        public override void Exit()
        {
            base.Exit();

            Perception.TargetLost -= OnTargetLost;
            Animation.SetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier, _defaultAnimationMultiplier);
        }

        private bool CanEngage()
        {
            return Owner.Combat && Owner.Combat.IsInRange(Config.Attack.Kind, Perception.Target);
        }

        private void OnTargetLost()
        {
            Perception.TargetLost -= OnTargetLost;
            ChangeTo(EnemyStateId.Idle);
        }
    }
}
