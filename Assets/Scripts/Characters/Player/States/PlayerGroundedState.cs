using Characters.Common.Animation;
using Characters.Player.Abilities;
using Characters.Player.Input;
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

            if (Controller.Input.WasPerformed(PlayerInputAction.Attack))
            {
                Controller.ChangeState(PlayerStateId.Attack);
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Jump))
            {
                Controller.ChangeState(PlayerStateId.Jump);
                return true;
            }

            return false;
        }

        public override bool TryFixedTransition()
        {
            if (base.TryFixedTransition())
            {
                return true;
            }

            if (Controller.Movement.IsFalling)
            {
                Controller.ChangeState(PlayerStateId.Fall);
                return true;
            }

            return false;
        }

        protected bool IsRunningIntoWall()
        {
            return Controller.Movement.IsWalled
                && Mathf.Approximately(Controller.Input.Move.x, Controller.Movement.FacingDirection);
        }
    }
}
