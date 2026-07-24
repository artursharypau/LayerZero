using Characters.Player.Abilities;
using Characters.Player.Abilities.Dash;
using Infrastructure.StateMachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerState
    {
        private readonly CountdownTimer _timer;

        private float _velocityX;
        private float _initialGravityScale;

        public PlayerDashState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.Dash)
        {
            _timer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            Controller.TryGetAbilityConfig(PlayerAbilityId.Dash, out PlayerDashAbilityConfig config);

            _timer.Start(config.Duration);
            _velocityX = Controller.MoveSpeed * config.SpeedMultiplier;
            _initialGravityScale = Controller.RB.gravityScale;

            Controller.TriggerAbility(PlayerAbilityId.Dash);
            Controller.RB.gravityScale = 0f;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.IsGrounded ? Controller.IdleState : Controller.WallSlideState);
                return true;
            }

            if (_timer.IsExpired)
            {
                if (Controller.IsWalled)
                {
                    FSM.ChangeState(Controller.WallSlideState);
                }
                else if (Controller.IsFalling)
                {
                    FSM.ChangeState(Controller.FallState);
                }
                else if (Controller.IsGrounded)
                {
                    FSM.ChangeState(Controller.IdleState);
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
