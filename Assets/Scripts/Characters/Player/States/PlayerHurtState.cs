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

        public override int Id => PlayerStateId.Hurt;

        protected override void OnHurtFinished()
        {
            if (!Movement.IsGrounded)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Fall);
                return;
            }

            if (Owner.Input.Move.x != 0f)
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Move);
            }
            else
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Idle);
            }
        }
    }
}
