using Characters.Common;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public class PlayerJumpAttackState : AttackState<PlayerController>
    {
        private bool _isGroundTouched;

        public PlayerJumpAttackState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimatorHashProvider.JumpAttack, AnimatorParameterType.Bool)
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

        public override void Update()
        {
            base.Update();

            if (Controller.IsGrounded && !_isGroundTouched)
            {
                _isGroundTouched = true;

                Anim.SetTrigger(PlayerAnimatorHashProvider.JumpAttackTrigger);
                Controller.SetHorizontalVelocity(0f);
            }
        }

        protected override void OnAttackFinished()
        {
            FSM.ChangeState(Controller.InputHandler.Move.x != 0f ? Controller.MoveState : Controller.IdleState);
        }
    }
}
