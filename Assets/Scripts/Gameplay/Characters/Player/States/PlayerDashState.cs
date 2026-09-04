using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Player.Abilities;
using LayerZero.Gameplay.Combat.Damage.Protections;

namespace LayerZero.Gameplay.Characters.Player.States
{
    internal sealed class PlayerDashState : PlayerState
    {
        private Countdown _timer;
        private ProtectionHandle _protection = ProtectionHandle.None;
        private float _speed;
        private float _defaultGravityScale;
        private bool _isDashing;

        public PlayerDashState(PlayerController owner)
            : base(owner)
        {
            On(() => !_isDashing, ResolveLocomotionState);
            On(() => _timer.IsExpired, ResolveLocomotionState);
            On(() => Movement.IsWalled, ResolveLocomotionState);
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

            _protection = Owner.DamageProtection.Apply(Protection.Invulnerability);

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

            Owner.DamageProtection.Remove(_protection);
            _protection = ProtectionHandle.None;
        }
    }
}
