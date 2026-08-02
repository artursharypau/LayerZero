using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallJumpState : PlayerInAirState
    {
        private readonly PlayerJumpAbilityConfig _config;
        private readonly CountdownTimer _moveLockTimer;

        public PlayerWallJumpState(PlayerController controller, PlayerJumpAbilityConfig config)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
            _config = config;
            _moveLockTimer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            _moveLockTimer.Start(_config.WallJumpMoveLockDuration);

            EnableMovement(false);
            Controller.Movement.SetVelocity(
                _config.WallJumpForce.x * -Controller.Movement.FacingDirection,
                _config.WallJumpForce.y,
                true);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.ChangeState(PlayerStateId.Jump);
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

            if (Controller.Movement.IsFalling)
            {
                Controller.ChangeState(PlayerStateId.Fall);
                return true;
            }

            if (Controller.Movement.IsWalled)
            {
                Controller.ChangeState(PlayerStateId.WallSlide);
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
                EnableMovement(true);
            }
        }
    }
}
