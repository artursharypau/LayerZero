using Characters.Common.States;
using Characters.Player.Abilities;
using Characters.Player.Input;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.WallSlide)
        {
            EnableMovement(false);
        }

        public override void Enter()
        {
            base.Enter();

            Controller.RefillChargeableAbility(PlayerAbilityId.Jump, 1);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.InputHandler.WasPerformed(PlayerInputAction.Jump))
            {
                Controller.ChangeState(StateId.WallJump);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
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

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleSliding();
        }

        private void HandleSliding()
        {
            float velocityY = Controller.InputHandler.Move.y < 0f
                ? Controller.VelocityY
                : Controller.VelocityY * Controller.WallSlideMultiplier;

            Controller.SetVelocity(0f, velocityY);
        }
    }
}
