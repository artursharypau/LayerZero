using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerHurtState : HurtStateBase<PlayerController>
    {
        public PlayerHurtState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Hurt)
        {
        }

        protected override void OnHurtFinished()
        {
            if (!Movement.IsGrounded)
            {
                ChangeTo<PlayerFallState>();
                return;
            }

            if (Owner.Input.Move.x != 0f)
            {
                ChangeTo<PlayerMoveState>();
            }
            else
            {
                ChangeTo<PlayerIdleState>();
            }
        }
    }
}
