using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;
using Characters.Player.Animation;
using Core.Utils;
using Systems.Damage.Resistance;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerState
    {
        private readonly PlayerDashAbilityConfig _config;
        private readonly CountdownTimer _timer;

        private float _velocityX;
        private float _initialGravityScale;

        private int _appliedResistanceIndex;

        public override int Id => (int)PlayerStateId.Dash;

        public PlayerDashState(PlayerController controller, PlayerDashAbilityConfig config)
            : base(controller, PlayerAnimatorHashProvider.Dash)
        {
            _config = config;
            _timer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _initialGravityScale = Controller.Movement.GravityScale;

            if (Controller.TryTriggerAbility(PlayerAbilityId.Dash))
            {
                _timer.Start(_config.Duration);
                _velocityX = Controller.MoveSpeed * _config.SpeedMultiplier;

                DamageResistance resistance = DamageResistance.Create().WithInvulnerability();
                _appliedResistanceIndex = Controller.DamageResistanceApplier.Apply(resistance);
            }
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Controller.Movement.IsWalled)
            {
                Controller.ChangeState(Controller.Movement.IsGrounded ? PlayerStateId.Idle : PlayerStateId.WallSlide);
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Controller.Movement.IsWalled)
                {
                    Controller.ChangeState(PlayerStateId.WallSlide);
                }
                else if (Controller.Movement.IsGrounded)
                {
                    Controller.ChangeState(PlayerStateId.Idle);
                }
                else
                {
                    Controller.ChangeState(PlayerStateId.Fall);
                }

                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _timer.Tick(Time.fixedDeltaTime);
            if (_timer.IsExpired)
            {
                Controller.Movement.SetGravityScale(_initialGravityScale);
                Controller.Movement.SetVelocity(0f, Controller.Movement.VelocityY);
            }
            else
            {
                Controller.Movement.SetGravityScale(0f);
                Controller.Movement.SetVelocity(_velocityX * Controller.Movement.FacingDirection, 0f);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Controller.Movement.SetGravityScale(_initialGravityScale);
            Controller.Movement.SetVelocity(0f, Controller.Movement.VelocityY);

            if (_appliedResistanceIndex != -1)
            {
                Controller.DamageResistanceApplier.Remove(_appliedResistanceIndex);
                _appliedResistanceIndex = -1;
            }
        }
    }
}
