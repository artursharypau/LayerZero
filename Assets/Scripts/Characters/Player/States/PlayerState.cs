using Core.Animation;
using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerState : State
    {
        private readonly CountdownTimer _dashTimer;

        protected PlayerController Controller { get; }

        protected PlayerState(
            StateMachine fsm,
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, new AnimatorContext(animParameterHash, animParameterType, controller.Anim))
        {
            Controller = controller;

            _dashTimer = new CountdownTimer();
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (!_dashTimer.IsRunning && !Controller.IsWalled && Controller.InputActions.Dash.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.DashState);
                _dashTimer.Start(Controller.DashDuration + Controller.DashCooldown);

                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            _dashTimer.Tick(Time.deltaTime);
        }
    }
}
