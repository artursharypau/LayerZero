using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        protected PlayerInAirState(PlayerController owner)
            : base(owner)
        {
            On(() => IsMovementEnabled && Input.WasPerformed(PlayerInputAction.Attack), PlayerStateId.JumpAttack);
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Dash), PlayerStateId.Dash);
        }

        protected bool IsMovementEnabled { get; set; } = true;

        public override void Enter()
        {
            base.Enter();

            IsMovementEnabled = true;
        }

        public override void Update()
        {
            base.Update();

            Animator.SetFloat(CommonAnimatorParameters.VelocityY, Movement.VelocityY);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsMovementEnabled && Input.Move.x != 0f)
            {
                Movement.SetVelocityX(Config.Movement.MoveSpeed * Config.Movement.InAirMoveMultiplier * Input.Move.x, true);
            }
        }
    }
}
