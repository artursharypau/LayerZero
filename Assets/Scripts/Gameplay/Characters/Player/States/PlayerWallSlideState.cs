using LayerZero.Gameplay.Characters.Player.Abilities;
using LayerZero.Gameplay.Characters.Player.Input;

namespace LayerZero.Gameplay.Characters.Player.States
{
    public sealed class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(PlayerController owner)
            : base(owner)
        {
            On(() => Input.WasPerformed(PlayerInputAction.Jump), PlayerStateId.WallJump);

            On(() => Movement.IsGrounded, TransitToIdle);
            On(() => !Movement.IsWalled && Movement.IsFalling, PlayerStateId.Fall);
        }

        public override int Id => PlayerStateId.WallSlide;

        public override void Enter()
        {
            base.Enter();

            IsMovementEnabled = false;

            Owner.Abilities.RefillTo(PlayerAbilityId.Jump, 1);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float velocityY = Input.Move.y < 0f
                ? Movement.VelocityY
                : Movement.VelocityY * Config.Movement.WallSlideMultiplier;

            Movement.SetVelocity(0f, velocityY);
        }

        private int TransitToIdle()
        {
            Movement.FaceTowards(Input.Move.x);
            return PlayerStateId.Idle;
        }
    }
}
