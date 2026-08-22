using LayerZero.Gameplay.Characters.Player.Abilities;

namespace LayerZero.Gameplay.Characters.Player.States
{
    public sealed class PlayerFallState : PlayerInAirState
    {
        private float _defaultGravityScale;

        public PlayerFallState(PlayerController owner)
            : base(owner)
        {
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Jump), PlayerStateId.Jump);

            On(() => Movement.IsGrounded, PlayerStateId.Idle);
            On(() => Movement.IsWalled, PlayerStateId.WallSlide);
        }

        public override int Id => PlayerStateId.Fall;

        public override void Enter()
        {
            base.Enter();

            _defaultGravityScale = Movement.GravityScale;
            Movement.SetGravityScale(_defaultGravityScale * Config.Jump.FallGravityMultiplier);
        }

        public override void Exit()
        {
            base.Exit();

            Movement.SetGravityScale(_defaultGravityScale);
        }
    }
}
