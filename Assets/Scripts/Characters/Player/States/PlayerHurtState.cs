using LayerZero.Characters.Common.States;
using LayerZero.Combat.Damage;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerHurtState : PlayerState, IStatePayload<DamageImpactInfo>
    {
        private readonly StunBehaviour _stun = new();

        private DamageImpactInfo _impact;
        private ResistanceHandle _resistance;

        public PlayerHurtState(PlayerController owner)
            : base(owner)
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
            _resistance = Owner.DamageResistances.Apply(DamageResistance.Invulnerability);

            _stun.Begin(impact.StunDuration);
            Movement.ApplyKnockback(impact.Knockback);
        }

        public override void Exit()
        {
            base.Exit();

            Owner.DamageResistances.Remove(_resistance);
            _resistance = ResistanceHandle.None;
        }
    }
}
