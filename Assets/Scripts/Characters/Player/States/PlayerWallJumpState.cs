using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallJumpState : PlayerInAirState
    {
        private readonly CountdownTimer _moveLockTimer;

        public PlayerWallJumpState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpFall)
        {
            _moveLockTimer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _moveLockTimer.Start(Controller.WallJumpMoveLockDuration);

            EnableInput(false);
            Controller.SetVelocity(Controller.WallJumpForce.x * -Controller.FacingDirection, Controller.WallJumpForce.y);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanJump())
            {
                FSM.ChangeState(Controller.JumpState);
                return true;
            }

            if (Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
                return true;
            }

            if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.WallSlideState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            if (_moveLockTimer.Tick(Time.deltaTime))
            {
                EnableInput(true);
            }
        }
    }
}
