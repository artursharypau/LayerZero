using LayerZero.Characters.Common.Animation;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyIdleState : EnemyGroundedState
    {
        private Countdown _timer;

        public EnemyIdleState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Idle)
        {
            On(() => _timer.IsExpired, EnemyStateId.Patrol);
        }

        public override int Id => EnemyStateId.Idle;

        public override void Enter()
        {
            base.Enter();

            _timer.Start(Config.Movement.IdleDuration);
            Movement.SetVelocityX(0f);
        }
    }
}
