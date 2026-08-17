using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Input;
using UnityEngine;

namespace LayerZero.Characters.Player.States
{
    public abstract class PlayerGroundedState : PlayerState
    {
        protected PlayerGroundedState(PlayerController owner)
            : base(owner)
        {
            On(() => Input.WasPerformed(PlayerInputAction.Attack), PlayerStateId.Attack);
            On(() => Input.WasPerformed(PlayerInputAction.Counterattack), PlayerStateId.Counterattack);
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Dash), PlayerStateId.Dash);
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Jump), PlayerStateId.Jump);

            OnFixed(() => Movement.IsFalling, PlayerStateId.Fall);
        }

        public override void Enter()
        {
            base.Enter();

            Owner.Abilities.Refill(PlayerAbilityId.Jump);
        }

        protected bool IsPushingIntoWall()
        {
            return Movement.IsWalled && Mathf.Approximately(Input.Move.x, Movement.FacingDirection);
        }
    }
}
