using LayerZero.Characters.Player.Abilities;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerJumpState : PlayerInAirState
    {
        public PlayerJumpState(PlayerController owner)
            : base(owner)
        {
            On(() => Movement.VelocityY <= 0f, PlayerStateId.Fall);
        }

        public override int Id => PlayerStateId.Jump;

        public override void Enter()
        {
            base.Enter();

            TryJump();
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
                Movement.SetVelocity(Movement.VelocityX, Config.Jump.Force);
            }
        }
    }
}
