using LayerZero.Characters.Player.Abilities;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerDashState : PlayerState
    {
        private Countdown _timer;
        private ResistanceHandle _resistance = ResistanceHandle.None;
        private float _speed;
        private float _defaultGravityScale;
        private bool _isDashing;

        public PlayerDashState(PlayerController owner)
            : base(owner)
        {
            On(() => !_isDashing, ResolveLocomotionState);
            On(() => _timer.IsExpired, ResolveLocomotionState);
            On(() => Movement.IsWalled, PlayerStateId.Idle);
        }

        public override int Id => PlayerStateId.Dash;

        public override void Enter()
        {
            base.Enter();

            _defaultGravityScale = Movement.GravityScale;
            _isDashing = Owner.Abilities.TryUse(PlayerAbilityId.Dash);

            if (!_isDashing)
            {
                return;
            }

            _timer.Start(Config.Dash.Duration);
            _speed = Config.Movement.MoveSpeed * Config.Dash.SpeedMultiplier;

            _resistance = Owner.DamageResistances.Apply(DamageResistance.Invulnerability);

            Movement.SetGravityScale(0f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Movement.SetVelocity(_speed * Movement.FacingDirection, 0f);
        }

        public override void Exit()
        {
            base.Exit();

            Movement.SetGravityScale(_defaultGravityScale);
            Movement.SetVelocityX(0f);

            Owner.DamageResistances.Remove(_resistance);
            _resistance = ResistanceHandle.None;
        }
    }
}
