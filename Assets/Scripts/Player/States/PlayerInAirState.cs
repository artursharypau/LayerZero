using Common;
using Common.Animations;

namespace Player.States
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

            if (_inputEnabled && Controller.InputActions.Attack.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.JumpAttackState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Anim.SetFloat(AnimationHashProvider.VelocityY, Controller.RB.linearVelocityY);

            HandleMove();
        }

        protected void EnableInput(bool enable)
        {
            _inputEnabled = enable;
        }

        private void HandleMove()
        {
            if (_inputEnabled && Controller.MoveInput.x != 0f)
            {
                Controller.SetVelocity(
                    Controller.MoveSpeed * Controller.InAirMoveMultiplier * Controller.MoveInput.x,
                    Controller.RB.linearVelocityY);
            }
        }
    }
}
