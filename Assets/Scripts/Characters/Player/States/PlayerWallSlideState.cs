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
        }

        public override int Id => PlayerStateId.WallSlide;

        public override void Enter()
        {
            base.Enter();

            SetMovementEnabled(false);

            Owner.Abilities.RefillTo(PlayerAbilityId.Jump, 1);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Input.WasPerformed(PlayerInputAction.Jump))
            {
                Owner.StateMachine.ChangeState(PlayerStateId.WallJump);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Movement.IsGrounded)
            {
                Movement.FaceTowards(Input.Move.x);
                Owner.StateMachine.ChangeState(PlayerStateId.Idle);
                return true;
            }

            if (!Movement.IsWalled && Movement.IsFalling)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Fall);
                return true;
            }

            return false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float velocityY = Input.Move.y < 0f
                ? Movement.VelocityY
                : Movement.VelocityY * Config.Movement.WallSlideMultiplier;

            Movement.SetVelocity(0f, velocityY);
        }
    }
}
