using Characters.Common.States;
using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerState
    {
        private readonly PlayerDashAbilityConfig _config;
        private readonly CountdownTimer _timer;

        private float _velocityX;
        private float _initialGravityScale;

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
                Controller.ChangeState(Controller.Movement.IsGrounded ? StateId.Idle : StateId.WallSlide);
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Controller.Movement.IsWalled)
                {
                    Controller.ChangeState(StateId.WallSlide);
                }
                else if (Controller.Movement.IsFalling)
                {
                    Controller.ChangeState(StateId.Fall);
                }
                else if (Controller.Movement.IsGrounded)
                {
                    Controller.ChangeState(StateId.Idle);
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
        }
    }
}
