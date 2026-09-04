using LayerZero.Core.StateMachine;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Combat.Damage;
using LayerZero.Gameplay.Combat.Damage.Protections;

namespace LayerZero.Gameplay.Characters.Player.States
{
    internal sealed class PlayerHurtState : PlayerState, IStatePayload<DamageImpactInfo>
    {
        private Countdown _stunTimer;
        private DamageImpactInfo _impact;
        private ProtectionHandle _protection;

        public PlayerHurtState(PlayerController owner)
            : base(owner)
        {
            On(() => _stunTimer.IsExpired, ResolveLocomotionState);
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
            _protection = Owner.DamageProtection.Apply(Protection.Invulnerability);

            _stunTimer.Start(impact.StunDuration);
            Movement.ApplyKnockback(impact.Knockback);
        }

        public override void Exit()
        {
            base.Exit();

            Owner.DamageProtection.Remove(_protection);
            _protection = ProtectionHandle.None;
        }
    }
}
