using Common;
using Common.Animations;
using UnityEngine;

namespace Player
{
    public abstract class PlayerState : State
    {
        private static float _dashCooldownTimer;

        protected PlayerController Controller { get; }

        protected PlayerState(
            StateMachine fsm,
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, new AnimationContext(animParameterHash, animParameterType, controller.Anim))
        {
            Controller = controller;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_dashCooldownTimer <= 0f && !Controller.IsWalled && Controller.InputActions.Dash.WasPerformedThisFrame())
            {
                _dashCooldownTimer = Controller.DashDuration + Controller.DashCooldown;
                FSM.ChangeState(Controller.DashState);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _dashCooldownTimer -= Time.deltaTime;
        }
    }
}
