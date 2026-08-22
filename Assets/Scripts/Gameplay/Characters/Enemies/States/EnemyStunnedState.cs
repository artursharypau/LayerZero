using LayerZero.Core.StateMachine;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Combat.Damage;

namespace LayerZero.Gameplay.Characters.Enemies.States
{
    public sealed class EnemyStunnedState : EnemyState, IStatePayload<DamageImpactInfo>
    {
        private Countdown _stunTimer;
        private DamageImpactInfo _impact;

        public EnemyStunnedState(EnemyController owner)
            : base(owner)
        {
            On(() => _stunTimer.IsExpired, ResolveRecoveryState);
        }

        public override int Id => EnemyStateId.Stunned;

        public void SetPayload(DamageImpactInfo payload)
        {
            _impact = payload;
        }

        public override void Enter()
        {
            base.Enter();

            if (Perception.IsTargetBehind)
            {
                Movement.Flip();
            }

            DamageImpactInfo impact = _impact;
            _impact = DamageImpactInfo.None;

            _stunTimer.Start(impact.StunDuration);
        }

        private int ResolveRecoveryState()
        {
            return Perception.HasTarget ? EnemyStateId.Chase : EnemyStateId.Patrol;
        }
    }
}
