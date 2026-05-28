using Common;

namespace Player.States
{
    public class PlayerJumpAttackState : PlayerState
    {
        private bool _isGroundTouched;

        public PlayerJumpAttackState(StateMachine fsm, PlayerController controller)
            : base(PlayerAnimationIdProvider.JumpAttack, fsm, controller)
        {
        }

        public override void Enter()
        {
            _isGroundTouched = false;

            Controller.AnimTriggers.AttackFinished += OnFinished;
            Controller.SetVelocity(
                Controller.JumpAttackVelocity.x * Controller.FacingDirection,
                Controller.JumpAttackVelocity.y);

            base.Enter();
        }

        public override void Update()
        {
            if (Controller.IsGrounded && !_isGroundTouched)
            {
                _isGroundTouched = true;

                Anim.SetTrigger(PlayerAnimationIdProvider.JumpAttackTrigger);
                Controller.SetVelocity(0f, Controller.RB.linearVelocityY);
            }

            base.Update();
        }

        public override void Exit()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            base.Exit();
        }

        private void OnFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;
            FSM.ChangeState(Controller.MoveInput.x != 0f ? Controller.MoveState : Controller.IdleState);
        }
    }
}
