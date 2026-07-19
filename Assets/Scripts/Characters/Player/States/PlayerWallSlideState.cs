using Core.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.WallSlide)
        {
            EnableInput(false);
        }

        public override void Enter()
        {
            base.Enter();

            Controller.ResetJump();
            Controller.ConsumeJump();
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.IsGrounded)
            {
                if (!Mathf.Approximately(Controller.FacingDirection, Controller.MoveInput.x))
                {
                    Controller.Flip();
                }

                FSM.ChangeState(Controller.IdleState);
                return true;
            }

            if (!Controller.IsWalled && Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
                return true;
            }

            if (Controller.InputActions.Jump.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.WallJumpState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            HandleSliding();
        }

        private void HandleSliding()
        {
            float velocityY = Controller.MoveInput.y < 0f
                ? Controller.RB.linearVelocityY
                : Controller.RB.linearVelocityY * Controller.WallSlideMultiplier;

            Controller.SetVelocity(Controller.MoveInput.x, velocityY);
        }
    }
}
