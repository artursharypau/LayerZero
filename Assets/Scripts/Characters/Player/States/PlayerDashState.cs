using Core.StateMachine;
using Core.Utils;
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

            _timer.Start(Controller.DashDuration);
            _velocityX = Controller.MoveSpeed * Controller.DashMultiplier;
            _initialGravityScale = Controller.RB.gravityScale;

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

            if (!_timer.IsRunning)
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
