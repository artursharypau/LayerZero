using Characters.Player.Abilities;
using Characters.Player.Input;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.WallSlide)
        {
            EnableInput(false);
        }

        public override void Enter()
        {
            base.Enter();

            Controller.RefillChargeableAbility(PlayerAbilityId.Jump);
            Controller.TriggerAbility(PlayerAbilityId.Jump);
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

                Controller.ChangeState(StateId.Idle);
                return true;
            }

            if (!Controller.IsWalled && Controller.IsFalling)
            {
                Controller.ChangeState(StateId.Fall);
                return true;
            }

            if (Controller.InputHandler.WasPerformed(PlayerInputAction.Jump))
            {
                Controller.ChangeState(StateId.WallJump);
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
