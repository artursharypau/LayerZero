using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    /// <summary>
    /// Gravity-free burst forward with invulnerability frames.
    /// The i-frames are released by handle, so an overlapping resistance never cancels the wrong one.
    /// </summary>
    public sealed class PlayerDashState : PlayerState
    {
        private readonly CountdownTimer _timer = new();

        private ResistanceHandle _invulnerability = ResistanceHandle.None;
        private float _speed;
        private float _defaultGravityScale;

        public PlayerDashState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.Dash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _defaultGravityScale = Movement.GravityScale;

            if (!Owner.Abilities.TryUse(PlayerAbilityId.Dash))
            {
                // Entered without the ability being available - bail out instead of hanging in the state.
                ChangeTo<PlayerIdleState>();
                return;
            }

            _timer.Start(Config.Dash.Duration);
            _speed = Config.Movement.MoveSpeed * Config.Dash.SpeedMultiplier;

            _invulnerability = Owner.Resistances.Apply(DamageResistance.Create().WithInvulnerability());

            Movement.SetGravityScale(0f);
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Movement.IsWalled)
            {
                ChangeTo<PlayerWallSlideState>();
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Movement.IsGrounded)
                {
                    ChangeTo<PlayerIdleState>();
                }
                else
                {
                    ChangeTo<PlayerFallState>();
                }

                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _timer.Tick(Time.fixedDeltaTime);
            Movement.SetVelocity(_speed * Movement.FacingDirection, 0f);
        }

        public override void Exit()
        {
            base.Exit();

            Movement.SetGravityScale(_defaultGravityScale);
            Movement.SetVelocityX(0f);

            Owner.Resistances.Remove(_invulnerability);
            _invulnerability = ResistanceHandle.None;
        }
    }
}
