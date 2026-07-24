using Characters.Player.Abilities;
using Characters.Player.Abilities.Jump;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.JumpFall)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Jump();
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.RB.linearVelocityY <= 0f)
            {
                Controller.ChangeState(StateId.Fall);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Jump();
        }

        private void Jump()
        {
            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.TryGetAbilityConfig(PlayerAbilityId.Jump, out PlayerJumpAbilityConfig config);
                Controller.TriggerAbility(PlayerAbilityId.Jump);

                Controller.SetVelocity(Controller.MoveSpeed * Controller.InputHandler.Move.x, config.Force);
            }
        }
    }
}
