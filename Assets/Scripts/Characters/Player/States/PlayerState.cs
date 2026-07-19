using Characters.Common;
using Core.Animation;
using Core.StateMachine;
using Core.Utils;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerState : CharacterState<PlayerController>
    {
        private readonly CountdownTimer _dashTimer;

        protected PlayerState(
            StateMachine fsm,
            PlayerController controller,
            int hash,
            AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(fsm, controller, hash, type)
        {
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
