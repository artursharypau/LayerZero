using Common;
using Common.Animations;

namespace Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        private bool _canControl;

        protected PlayerInAirState(
            StateMachine fsm,
            PlayerController controller,
            bool canControl,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, controller, animParameterHash, animParameterType)
        {
            _canControl = canControl;
        }

        public override void Update()
        {
            Anim.SetFloat(AnimationHashProvider.VelocityY, Controller.RB.linearVelocityY);

            if (_canControl)
            {
                HandleControl();
            }

            base.Update();
        }

        protected void SetCanControl(bool canControl)
        {
            _canControl = canControl;
        }

        private void HandleControl()
        {
            if (Controller.InputActions.Attack.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.JumpAttackState);
            }

            if (Controller.MoveInput.x != 0f)
            {
                Controller.SetVelocity(
                    Controller.MoveSpeed * Controller.InAirMoveMultiplier * Controller.MoveInput.x,
                    Controller.RB.linearVelocityY);
            }
        }
    }
}
