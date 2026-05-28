using Common;
using UnityEngine;

namespace Player.States
{
    public class PlayerDashState : PlayerState
    {
        private float _timer;
        private float _velocityX;
        private float _initialGravityScale;

        public PlayerDashState(StateMachine fsm, PlayerController controller)
            : base(PlayerAnimationIdProvider.Dash, fsm, controller)
        {
        }

        public override void Enter()
        {
            _timer = Controller.DashDuration;
            _velocityX = Controller.MoveSpeed * Controller.DashMultiplier;
            _initialGravityScale = Controller.RB.gravityScale;

            Controller.RB.gravityScale = 0f;

            base.Enter();
        }

        public override void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                SwitchState();
            }
            else
            {
                HandleDash();
            }

            base.Update();
        }

        public override void Exit()
        {
            Controller.SetVelocity(0f, 0f);
            Controller.RB.gravityScale = _initialGravityScale;

            base.Exit();
        }

        private void SwitchState()
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
