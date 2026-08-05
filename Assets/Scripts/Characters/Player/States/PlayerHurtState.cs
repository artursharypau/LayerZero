using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Combat.Damage;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerHurtState : PlayerState, IStatePayload<DamageImpactInfo>
    {
        private readonly StunBehaviour _stun = new();

        private DamageImpactInfo _impact;

        public PlayerHurtState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Hurt)
        {
            On(() => _stun.IsFinished, ResolveLocomotionState);
        }

        public override int Id => PlayerStateId.Hurt;

        public void SetPayload(DamageImpactInfo payload)
        {
            _impact = payload;
        }

        public override void Enter()
        {
            base.Enter();

            DamageImpactInfo impact = _impact;
            _impact = DamageImpactInfo.None;

            _stun.Begin(Movement, impact);
        }
    }
}
