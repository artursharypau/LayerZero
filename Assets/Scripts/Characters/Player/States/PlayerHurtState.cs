using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Player.States
{
    public class PlayerHurtState : HurtState<PlayerController>
    {
        public PlayerHurtState(PlayerController controller)
            : base(controller, AnimatorHashProvider.Hurt, AnimatorParameterType.Bool)
        {
        }

        protected override void OnHurtFinished()
        {
            if (!Controller.Movement.IsGrounded)
            {
                Controller.ChangeState(PlayerStateId.Fall);
                return;
            }

            Controller.ChangeState(Controller.Input.Move.x != 0f ? PlayerStateId.Move : PlayerStateId.Idle);
        }
    }
}
