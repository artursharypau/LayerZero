using Characters.Player.Abilities;
using Characters.Player.Abilities.Jump;
using Infrastructure.StateMachine;
using Infrastructure.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerWallJumpState : PlayerInAirState
    {
        private readonly CountdownTimer _moveLockTimer;

        public PlayerWallJumpState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
            _moveLockTimer = new CountdownTimer();
        }

        public override void Enter()
        {
            base.Enter();

            Controller.TryGetAbilityConfig(PlayerAbilityId.Jump, out PlayerJumpAbilityConfig config);

            _moveLockTimer.Start(config.WallJumpMoveLockDuration);

            EnableInput(false);
            Controller.SetVelocity(config.WallJumpForce.x * -Controller.FacingDirection, config.WallJumpForce.y);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.ChangeState(StateId.Jump);
                return true;
            }

            if (Controller.IsFalling)
            {
                Controller.ChangeState(StateId.Fall);
                return true;
            }

            if (Controller.IsWalled)
            {
                Controller.ChangeState(StateId.WallSlide);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _moveLockTimer.Tick(Time.deltaTime);
            if (_moveLockTimer.IsExpired)
            {
                EnableInput(true);
            }
        }
    }
}
