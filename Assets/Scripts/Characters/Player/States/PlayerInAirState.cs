using Characters.Player.Input;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        private bool _inputEnabled;

        protected PlayerInAirState(
            StateMachine fsm,
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, controller, animParameterHash, animParameterType)
        {
            _inputEnabled = true;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_inputEnabled && Controller.InputHandler.WasPerformed(PlayerInputAction.Attack))
            {
                FSM.ChangeState(Controller.JumpAttackState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimatorHashProvider.VelocityY, Controller.RB.linearVelocityY);

            HandleMove();
        }

        protected void EnableInput(bool enable)
        {
            _inputEnabled = enable;
        }

        private void HandleMove()
        {
            if (_inputEnabled && Controller.InputHandler.Move.x != 0f)
            {
                Controller.SetHorizontalVelocity(Controller.MoveSpeed * Controller.InAirMoveMultiplier * Controller.InputHandler.Move.x);
            }
        }
    }
}
