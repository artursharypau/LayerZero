using Characters.Player.Abilities;
using Characters.Player.Animation;
using Characters.Player.Input;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallSlideState : PlayerInAirState
    {
        public override int Id => (int)PlayerStateId.WallSlide;

        public PlayerWallSlideState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.WallSlide)
        {
        }

        public override void Enter()
        {
            base.Enter();

            EnableMovement(false);
            Controller.RefillChargeableAbility(PlayerAbilityId.Jump, 1);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.Input.WasPerformed(PlayerInputAction.Jump))
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
                if (!Mathf.Approximately(Controller.Movement.FacingDirection, Controller.Input.Move.x))
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
            float velocityY = Controller.Input.Move.y < 0f
                ? Controller.Movement.VelocityY
                : Controller.Movement.VelocityY * Controller.WallSlideMultiplier;

            Controller.Movement.SetVelocity(0f, velocityY);
        }
    }
}
