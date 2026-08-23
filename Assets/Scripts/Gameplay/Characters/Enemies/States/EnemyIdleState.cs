using LayerZero.Core.Timing;

namespace LayerZero.Gameplay.Characters.Enemies.States
{
    internal sealed class EnemyIdleState : EnemyGroundedState
    {
        private Countdown _timer;

        public EnemyIdleState(EnemyController owner)
            : base(owner)
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
