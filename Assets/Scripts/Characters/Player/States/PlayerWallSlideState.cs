using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerWallSlideState : PlayerInAirState
    {
        public PlayerWallSlideState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.WallSlide)
        {
            On(() => Input.WasPerformed(PlayerInputAction.Jump), PlayerStateId.WallJump);

            OnFixed(() => Movement.IsGrounded, TransitToIdle);
            OnFixed(() => !Movement.IsWalled && Movement.IsFalling, PlayerStateId.Fall);
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
