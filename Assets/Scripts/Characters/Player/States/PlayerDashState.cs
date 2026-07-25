using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;
using Infrastructure.StateMachine;
using Infrastructure.Utils;
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

            _initialGravityScale = Controller.RB.gravityScale;

            if (Controller.TryTriggerAbility(PlayerAbilityId.Dash))
            {
                _timer.Start(_config.Duration);
                _velocityX = Controller.MoveSpeed * _config.SpeedMultiplier;

                Controller.RB.gravityScale = 0f;
            }
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.IsWalled)
            {
                Controller.ChangeState(Controller.IsGrounded ? StateId.Idle : StateId.WallSlide);
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Controller.IsWalled)
                {
                    Controller.ChangeState(StateId.WallSlide);
                }
                else if (Controller.IsFalling)
                {
                    Controller.ChangeState(StateId.Fall);
                }
                else if (Controller.IsGrounded)
                {
                    Controller.ChangeState(StateId.Idle);
                }

                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _timer.Tick(Time.deltaTime);
            if (_timer.IsExpired)
            {
                Controller.RB.gravityScale = _initialGravityScale;
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            }
            else
            {
                Controller.SetVelocity(_velocityX * Controller.FacingDirection, 0f);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            Controller.RB.gravityScale = _initialGravityScale;
        }
    }
}
