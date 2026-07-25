using Characters.Common.Animation;
using Characters.Common.States;
using Characters.Player.Input;

namespace Characters.Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        private bool _inputEnabled;

        protected PlayerInAirState(
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(controller, animParameterHash, animParameterType)
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
                Controller.ChangeState(StateId.JumpAttack);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimatorHashProvider.VelocityY, Controller.VelocityY);

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
