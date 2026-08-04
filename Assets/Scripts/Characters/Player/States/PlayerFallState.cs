using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Animation;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerFallState : PlayerInAirState
    {
        public PlayerFallState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.JumpFall)
        {
        }

        public override int Id => PlayerStateId.Fall;

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Owner.Abilities.CanUse(PlayerAbilityId.Jump))
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Jump);
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
                Owner.StateMachine.ChangeState(PlayerStateId.Idle);
                return true;
            }

            if (Movement.IsWalled)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.WallSlide);
                return true;
            }

            return false;
        }
    }
}
