using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;

namespace LayerZero.Characters.Player.States
{
    /// <summary>
    /// Rising part of a jump. Re-checks the jump ability every physics step so a buffered
    /// second jump turns into a double jump without leaving the state.
    /// </summary>
    public sealed class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.JumpFall)
        {
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

            if (Movement.VelocityY <= 0f)
            {
                ChangeTo<PlayerFallState>();
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
