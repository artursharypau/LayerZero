using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Enemies.Animation;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>
    /// Closes on the target and hands over to the attack state once the combat system reports
    /// the target is reachable.
    /// <para>
    /// Both hooks - <see cref="GetMoveDirection" /> and <see cref="CanEngage" /> - exist so a
    /// ranged archetype can reuse the whole loop and only change how it positions itself.
    /// </para>
    /// </summary>
    public class EnemyChaseState : EnemyState
    {
        private float _defaultAnimationMultiplier;

        public EnemyChaseState(EnemyController owner)
            : base(owner, EnemyAnimatorParameters.Chase)
        {
        }

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
                ChangeTo<EnemyAttackState>();
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

            Movement.SetVelocityX(Config.Movement.MoveSpeed * Config.Chase.SpeedMultiplier * GetMoveDirection());
        }

        public override void Exit()
        {
            base.Exit();

            Perception.TargetLost -= OnTargetLost;
            Animation.SetFloat(EnemyAnimatorParameters.ChaseAnimationMultiplier, _defaultAnimationMultiplier);
        }

        /// <summary>Which way to move this step. Melee walks straight at the target.</summary>
        protected virtual float GetMoveDirection()
        {
            return Perception.DirectionToTarget;
        }

        /// <summary>Whether the enemy may start its attack right now.</summary>
        protected virtual bool CanEngage()
        {
            return Owner.Combat && Owner.Combat.IsInRange(Config.Attack.Kind, Perception.Target);
        }

        private void OnTargetLost()
        {
            Perception.TargetLost -= OnTargetLost;
            ChangeTo<EnemyIdleState>();
        }
    }
}
