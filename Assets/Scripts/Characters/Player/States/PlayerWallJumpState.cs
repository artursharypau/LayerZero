using Core.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallJumpState : PlayerInAirState
    {
        private float _moveLockTimer;

        public PlayerWallJumpState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpFall)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _moveLockTimer = Controller.WallJumpMoveLockDuration;

            EnableInput(false);
            Controller.SetVelocity(
                Controller.WallJumpForce.x * -Controller.FacingDirection,
                Controller.WallJumpForce.y);
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

            _moveLockTimer -= Time.deltaTime;
            if (_moveLockTimer <= 0f)
            {
                EnableInput(true);
            }
        }
    }
}
