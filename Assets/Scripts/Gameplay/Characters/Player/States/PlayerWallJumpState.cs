using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Player.Abilities;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Player.States
{
    internal sealed class PlayerWallJumpState : PlayerInAirState
    {
        private Countdown _moveLock;

        public PlayerWallJumpState(PlayerController owner)
            : base(owner)
        {
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Jump), PlayerStateId.Jump);

            On(() => Movement.IsFalling, PlayerStateId.Fall);
            On(() => Movement.IsWalled, PlayerStateId.WallSlide);
        }

        public override int Id => PlayerStateId.WallJump;

        public override void Enter()
        {
            base.Enter();

            _moveLock.Start(Config.Jump.WallJumpMoveLockDuration);
            IsMovementEnabled = false;

            Vector2 force = Config.Jump.WallJumpForce;
            Movement.SetVelocity(force.x * -Movement.FacingDirection, force.y, true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!IsMovementEnabled && _moveLock.IsExpired)
            {
                IsMovementEnabled = true;
            }
        }
    }
}
