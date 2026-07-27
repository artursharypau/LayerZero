using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Player.States
{
    public class PlayerJumpAttackState : AttackState<PlayerController>
    {
        private bool _isGroundTouched;

        public PlayerJumpAttackState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.JumpAttack, AnimatorParameterType.Bool)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _isGroundTouched = false;

            Controller.SetVelocity(
                Controller.JumpAttackVelocity.x * Controller.FacingDirection,
                Controller.JumpAttackVelocity.y);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Controller.IsGrounded && !_isGroundTouched)
            {
                _isGroundTouched = true;

                Anim.SetTrigger(PlayerAnimatorHashProvider.JumpAttackTrigger);
                Controller.SetVelocityX(0f);
            }
        }

        protected override void OnAttackFinished()
        {
            Controller.ChangeState(Controller.InputHandler.Move.x != 0f ? StateId.Move : StateId.Idle);
        }
    }
}
