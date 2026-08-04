using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.JumpFall)
        {
        }

        public override int Id => PlayerStateId.Jump;

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

            if (Movement.VelocityY <= 0f)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Fall);
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
            if (Owner.Abilities.TryUse(PlayerAbilityId.Jump))
            {
                Movement.SetVelocity(Config.Movement.MoveSpeed * Input.Move.x, Config.Jump.Force, true);
            }
        }
    }
}
