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
                Controller.ChangeState(PlayerStateId.WallJump);
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

            if (Controller.Movement.IsGrounded)
            {
                if (!Mathf.Approximately(Controller.Movement.FacingDirection, Controller.InputHandler.Move.x))
                {
                    Controller.Movement.Flip();
                }

                Controller.ChangeState(PlayerStateId.Idle);
                return true;
            }

            if (!Controller.Movement.IsWalled && Controller.Movement.IsFalling)
            {
                Controller.ChangeState(PlayerStateId.Fall);
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
                ? Controller.Movement.VelocityY
                : Controller.Movement.VelocityY * Controller.WallSlideMultiplier;

            Controller.Movement.SetVelocity(0f, velocityY);
        }
    }
}
