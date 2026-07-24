using Infrastructure.Animation;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(
            StateMachine fsm,
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, controller, animParameterHash, animParameterType)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.JumpAbility.Reset();
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.InputHandler.WasAttackPerformed())
            {
                FSM.ChangeState(Controller.AttackState);
                return true;
            }

            if (Controller.CanJump())
            {
                FSM.ChangeState(Controller.JumpState);
                return true;
            }

            if (Controller.IsFalling)
            {
                FSM.ChangeState(Controller.FallState);
                return true;
            }

            return false;
        }

        protected bool IsRunningIntoWall()
        {
            return Controller.IsWalled
                   && Mathf.Approximately(Controller.InputHandler.Move.x, Controller.FacingDirection);
        }
    }
}
