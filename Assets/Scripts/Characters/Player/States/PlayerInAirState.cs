using Characters.Common.Animation;
using Characters.Common.States;
using Characters.Player.Input;

namespace Characters.Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        private bool _movementEnabled;

        protected PlayerInAirState(
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(controller, animParameterHash, animParameterType)
        {
            _movementEnabled = true;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_movementEnabled && Controller.InputHandler.WasPerformed(PlayerInputAction.Attack))
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
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleMove();
        }

        protected void EnableMovement(bool enable)
        {
            _movementEnabled = enable;
        }

        private void HandleMove()
        {
            if (_movementEnabled && Controller.InputHandler.Move.x != 0f)
            {
                Controller.SetVelocityX(
                    Controller.MoveSpeed * Controller.InAirMoveMultiplier * Controller.InputHandler.Move.x,
                    true);
            }
        }
    }
}
