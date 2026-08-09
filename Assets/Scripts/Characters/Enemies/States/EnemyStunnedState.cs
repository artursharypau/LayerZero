using LayerZero.Characters.Common.States;
using LayerZero.Combat.Damage;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyStunnedState : EnemyState, IStatePayload<DamageImpactInfo>
    {
        private readonly StunBehaviour _stun = new();

        private DamageImpactInfo _impact;

        public EnemyStunnedState(EnemyController owner)
            : base(owner)
        {
            On(() => _stun.IsFinished, ResolveRecoveryState);
        }

        public override int Id => EnemyStateId.Stunned;

        public void SetPayload(DamageImpactInfo payload)
        {
            _impact = payload;
        }

        public override void Enter()
        {
            base.Enter();

            DamageImpactInfo impact = _impact;
            _impact = DamageImpactInfo.None;

            _stun.Begin(impact.StunDuration);
        }

        private int ResolveRecoveryState()
        {
            return Perception.HasTarget ? EnemyStateId.Chase : EnemyStateId.Patrol;
        }
    }
}
