using Core.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDashState : PlayerState
    {
        private float _timer;
        private float _velocityX;
        private float _initialGravityScale;

        public PlayerDashState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.Dash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _timer = Controller.DashDuration;
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

            if (_timer <= 0f)
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

            _timer -= Time.deltaTime;
            HandleDash();
        }

        public override void Exit()
        {
            base.Exit();

            Controller.SetVelocity(0f, 0f);
            Controller.RB.gravityScale = _initialGravityScale;
        }

        private void HandleDash()
        {
            if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.IsGrounded ? Controller.IdleState : Controller.WallSlideState);
            }
            else
            {
                Controller.SetVelocity(_velocityX * Controller.FacingDirection, 0f);
            }
        }
    }
}
