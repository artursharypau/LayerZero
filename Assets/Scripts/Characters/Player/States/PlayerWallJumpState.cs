using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Core.Timing;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerWallJumpState : PlayerInAirState
    {
        private readonly CountdownTimer _moveLockTimer = new();

        public PlayerWallJumpState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.JumpFall)
        {
        }

        public override int Id => PlayerStateId.WallJump;

        public override void Enter()
        {
            base.Enter();

            _moveLockTimer.Start(Config.Jump.WallJumpMoveLockDuration);
            SetMovementEnabled(false);

            Vector2 force = Config.Jump.WallJumpForce;
            Movement.SetVelocity(force.x * -Movement.FacingDirection, force.y, true);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Owner.Abilities.CanUse(PlayerAbilityId.Jump))
            {
                ChangeTo(PlayerStateId.Jump);
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

            if (Movement.IsFalling)
            {
                ChangeTo(PlayerStateId.Fall);
                return true;
            }

            if (Movement.IsWalled)
            {
                ChangeTo(PlayerStateId.WallSlide);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _moveLockTimer.Tick(Time.fixedDeltaTime);
            if (_moveLockTimer.IsExpired)
            {
                SetMovementEnabled(true);
            }
        }
    }
}
