using Infrastructure.StateMachine;
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

            Controller.JumpAbility.Reset();
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
                if (!Mathf.Approximately(Controller.FacingDirection, Controller.InputHandler.Move.x))
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

            if (Controller.InputHandler.WasJumpPerformed())
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
            float velocityY = Controller.InputHandler.Move.y < 0f
                ? Controller.RB.linearVelocityY
                : Controller.RB.linearVelocityY * Controller.WallSlideMultiplier;

            Controller.SetVelocity(Controller.InputHandler.Move.x, velocityY);
        }
    }
}
