using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Input;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    /// <summary>Everything that is true while standing on the ground: jump refill, attack, jump, falling off.</summary>
    public abstract class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(PlayerController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Owner.Abilities.Refill(PlayerAbilityId.Jump);
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Input.WasPerformed(PlayerInputAction.Attack))
            {
                ChangeTo<PlayerAttackState>();
                return true;
            }

            if (Owner.Abilities.CanUse(PlayerAbilityId.Jump))
            {
                ChangeTo<PlayerJumpState>();
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

            if (Movement.IsFalling)
            {
                ChangeTo<PlayerFallState>();
                return true;
            }

            return false;
        }

        protected bool IsPushingIntoWall()
        {
            return Movement.IsWalled && Mathf.Approximately(Input.Move.x, Movement.FacingDirection);
        }
    }
}
