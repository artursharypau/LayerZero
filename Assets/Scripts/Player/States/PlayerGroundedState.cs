using Common;
using UnityEngine;

namespace Player.States
{
    public abstract class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(int animEntryId, StateMachine fsm, PlayerController controller)
            : base(animEntryId, fsm, controller)
        {
        }

        public override void Enter()
        {
            Controller.ResetJump();

            base.Enter();
        }

        public override void Update()
        {
            if (Controller.InputActions.BasicAttack.WasPerformedThisFrame())
            {
                FSM.ChangeState(Controller.BasicAttackState);
            }
            else if (Controller.CanJump())
            {
                FSM.ChangeState(Controller.JumpState);
            }
            else if (Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
            }

            base.Update();
        }

        protected bool IsRunningIntoWall()
        {
            return Controller.IsWalled && Mathf.Approximately(Controller.MoveInput.x, Controller.FacingDirection);
        }
    }
}
