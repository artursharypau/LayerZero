using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Combat.Damage;
using LayerZero.Core.Diagnostics;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Enemies.States
{
    public sealed class EnemyHurtState : EnemyState, IStatePayload<DamageImpactInfo>
    {
        private readonly StunBehaviour _stun = new();

        private DamageImpactInfo _impact;

        public EnemyHurtState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Hurt)
        {
            On(() => _stun.IsFinished, ResolveRecoveryState);
        }

        public override int Id => EnemyStateId.Hurt;

        public void SetPayload(DamageImpactInfo payload)
        {
            _impact = payload;
        }

        public override void Enter()
        {
            base.Enter();

            GameLog.Info(this, "Entering hurt state");

            DamageImpactInfo impact = _impact;
            _impact = DamageImpactInfo.None;

            _stun.Begin(Movement, impact);
        }

        private int ResolveRecoveryState()
        {
            return Perception.HasTarget ? EnemyStateId.Chase : EnemyStateId.Patrol;
        }
    }
}
