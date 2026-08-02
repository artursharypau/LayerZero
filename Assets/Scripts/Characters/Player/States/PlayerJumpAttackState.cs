using Characters.Common.Animation;
using Characters.Common.States;
using Characters.Player.Animation;

namespace Characters.Player.States
{
    public class PlayerJumpAttackState : AttackState<PlayerController>
    {
        private bool _isGroundTouched;

        public override int Id => (int)PlayerStateId.JumpAttack;

        public PlayerJumpAttackState(PlayerController controller)
            : base(controller, PlayerAnimatorHashProvider.JumpAttack, AnimatorParameterType.Bool)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _isGroundTouched = false;

            Controller.Combat.SetActiveAttackDefinition(Controller.JumpAttackDefinition);
            Controller.Movement.SetVelocity(
                Controller.JumpAttackVelocity.x * Controller.Movement.FacingDirection,
                Controller.JumpAttackVelocity.y);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Controller.Movement.IsGrounded && !_isGroundTouched)
            {
                _isGroundTouched = true;

                Anim.SetTrigger(PlayerAnimatorHashProvider.JumpAttackTrigger);
                Controller.Movement.SetVelocityX(0f);
            }
        }

        protected override void OnAttackFinished()
        {
            Controller.ChangeState(Controller.Input.Move.x != 0f ? PlayerStateId.Move : PlayerStateId.Idle);
        }
    }
}
