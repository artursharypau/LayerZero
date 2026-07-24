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

            Controller.DashAbility.Trigger();
            Controller.RB.gravityScale = 0f;

            _timer.Start(Controller.DashAbility.Duration);
            _velocityX = Controller.MoveSpeed * Controller.DashAbility.SpeedMultiplier;
            _initialGravityScale = Controller.RB.gravityScale;
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
                else
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
            Controller.SetVelocity(_velocityX * Controller.FacingDirection, 0f);
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, 0f);
            Controller.RB.gravityScale = _initialGravityScale;
        }
    }
}
