using Common;
using UnityEngine;

namespace Player
{
    public abstract class PlayerState : StateBase
    {
        private static float _dashCooldownTimer;

        protected PlayerController Controller { get; }

        protected PlayerState(int animEntryId, StateMachine fsm, PlayerController controller)
            : base(animEntryId, fsm, controller.Anim)
        {
            Controller = controller;
        }

        public override void Update()
        {
            base.Update();

            if (_dashCooldownTimer <= 0f && !Controller.IsWalled && Controller.InputActions.Dash.WasPerformedThisFrame())
            {
                _dashCooldownTimer = Controller.DashDuration + Controller.DashCooldown;
                FSM.ChangeState(Controller.DashState);
            }
            else
            {
                _dashCooldownTimer -= Time.deltaTime;
            }
        }
    }
}
