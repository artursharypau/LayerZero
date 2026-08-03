using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Characters.Player.Input;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.WallSlide)
        {
        }

        public override void Enter()
        {
            base.Enter();

            SetMovementEnabled(false);

            // Exactly one charge: the wall jump itself, no free double jump off a wall.
            Owner.Abilities.RefillTo(PlayerAbilityId.Jump, 1);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Input.WasPerformed(PlayerInputAction.Jump))
            {
                ChangeTo<PlayerWallJumpState>();
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

            if (Movement.IsGrounded)
            {
                Movement.FaceTowards(Input.Move.x);
                ChangeTo<PlayerIdleState>();
                return true;
            }

            if (!Movement.IsWalled && Movement.IsFalling)
            {
                ChangeTo<PlayerFallState>();
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Holding "down" cancels the slow-down and drops at full speed.
            float velocityY = Input.Move.y < 0f
                ? Movement.VelocityY
                : Movement.VelocityY * Config.Movement.WallSlideMultiplier;

            Movement.SetVelocity(0f, velocityY);
        }
    }
}
