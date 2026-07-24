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

        public PlayerWallJumpState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpFall)
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
                FSM.ChangeState(Controller.JumpState);
                return true;
            }

            if (Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
                return true;
            }

            if (Controller.IsWalled)
            {
                FSM.ChangeState(Controller.WallSlideState);
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
