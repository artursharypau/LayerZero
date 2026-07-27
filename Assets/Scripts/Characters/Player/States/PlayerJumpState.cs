using Characters.Common.States;
using Characters.Player.Abilities;
using Characters.Player.Abilities.Config;

namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerInAirState
    {
        private readonly PlayerJumpAbilityConfig _config;

        public PlayerJumpState(PlayerController controller, PlayerJumpAbilityConfig config)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
            _config = config;
        }

        public override void Enter()
        {
            base.Enter();

            Jump();
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Controller.VelocityY <= 0f)
            {
                Controller.ChangeState(StateId.Fall);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Jump();
        }

        private void Jump()
        {
            if (Controller.TryTriggerAbility(PlayerAbilityId.Jump))
            {
                Controller.SetVelocity(Controller.MoveSpeed * Controller.InputHandler.Move.x, _config.Force, true);
            }
        }
    }
}
