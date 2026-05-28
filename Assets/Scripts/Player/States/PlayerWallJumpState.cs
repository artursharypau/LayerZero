using Common;
using UnityEngine;

namespace Player.States
{
    public class PlayerWallJumpState : PlayerInAirState
    {
        private float _moveLockTimer;

        public PlayerWallJumpState(StateMachine fsm, PlayerController controller)
            : base(true, PlayerAnimationIdProvider.JumpFall, fsm, controller)
        {
        }

        public override void Enter()
        {
            _moveLockTimer = Controller.WallJumpMoveLockDuration;

            SetCanControl(false);
            Controller.SetVelocity(Controller.WallJumpForce.x * -Controller.FacingDirection, Controller.WallJumpForce.y);

            base.Enter();
        }

        public override void Update()
        {
            _moveLockTimer -= Time.deltaTime;

            if (Controller.CanJump())
            {
                FSM.ChangeState(Controller.JumpState);
            }
            else if (Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
            }
            else if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.WallSlideState);
            }
            else if (_moveLockTimer <= 0f)
            {
                SetCanControl(true);
            }

            base.Update();
        }
    }
}
