using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Combat.Damage.Resistance;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
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

        public override int Id => PlayerStateId.Dash;

        public override void Enter()
        {
            base.Enter();

            _defaultGravityScale = Movement.GravityScale;

            if (!Owner.Abilities.TryUse(PlayerAbilityId.Dash))
            {
                ChangeTo(PlayerStateId.Idle);
                return;
            }

            _timer.Start(Config.Dash.Duration);
            _speed = Config.Movement.MoveSpeed * Config.Dash.SpeedMultiplier;

            _invulnerability = Owner.DamageResistances.Apply(DamageResistance.Create().WithInvulnerability());

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
                ChangeTo(PlayerStateId.WallSlide);
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Movement.IsGrounded)
                {
                    ChangeTo(PlayerStateId.Idle);
                }
                else
                {
                    ChangeTo(PlayerStateId.Fall);
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

            Owner.DamageResistances.Remove(_invulnerability);
            _invulnerability = ResistanceHandle.None;
        }
    }
}
