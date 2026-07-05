using Common;

namespace Player.States
{
    public class PlayerJumpAttackState : PlayerState
    {
        private bool _isGroundTouched;

        public PlayerJumpAttackState(StateMachine fsm, PlayerController controller)
            : base(fsm, controller, PlayerAnimationHashProvider.JumpAttack)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _isGroundTouched = false;

            Controller.AnimTriggers.AttackFinished += OnFinished;
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

                Anim.SetTrigger(PlayerAnimationHashProvider.JumpAttackTrigger);
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            }
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AnimTriggers.AttackFinished -= OnFinished;
        }

        private void OnFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;
            FSM.ChangeState(Controller.MoveInput.x != 0f ? Controller.MoveState : Controller.IdleState);
        }
    }
}
