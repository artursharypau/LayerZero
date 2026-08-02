using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;
using Characters.Player.Animation;

namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerInAirState
    {
        private readonly PlayerJumpAbilityConfig _config;

        public override int Id => (int)PlayerStateId.Jump;

        public PlayerJumpState(PlayerController controller, PlayerJumpAbilityConfig config)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
            _config = config;
        }

        public override void Enter()
        {
            base.Enter();

            TryJump();
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Controller.Movement.VelocityY <= 0f)
            {
                Controller.ChangeState(PlayerStateId.Fall);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            TryJump();
        }

        private void TryJump()
        {
            if (Controller.TryTriggerAbility(PlayerAbilityId.Jump))
            {
                Controller.Movement.SetVelocity(Controller.MoveSpeed * Controller.Input.Move.x, _config.Force, true);
            }
        }
    }
}
