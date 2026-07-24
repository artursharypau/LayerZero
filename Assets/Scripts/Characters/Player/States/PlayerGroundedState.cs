using Characters.Player.Abilities;
using Characters.Player.Input;
using Infrastructure.Animation;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Characters.Player.States
{
    public abstract class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(
            PlayerController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(controller, animParameterHash, animParameterType)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.RefillChargeableAbility(PlayerAbilityId.Jump);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.InputHandler.WasPerformed(PlayerInputAction.Attack))
            {
                Controller.ChangeState(StateId.Attack);
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.ChangeState(StateId.Jump);
                return true;
            }

            if (Controller.IsFalling)
            {
                Controller.ChangeState(StateId.Fall);
                return true;
            }

            return false;
        }

        protected bool IsRunningIntoWall()
        {
            return Controller.IsWalled && Mathf.Approximately(Controller.InputHandler.Move.x, Controller.FacingDirection);
        }
    }
}
