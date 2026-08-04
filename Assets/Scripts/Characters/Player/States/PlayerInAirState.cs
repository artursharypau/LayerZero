using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    public abstract class PlayerInAirState : PlayerState
    {
        protected PlayerInAirState(PlayerController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        protected bool IsMovementEnabled { get; private set; } = true;

        public override void Enter()
        {
            base.Enter();

            IsMovementEnabled = true;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (IsMovementEnabled && Input.WasPerformed(PlayerInputAction.Attack))
            {
                ChangeTo(PlayerStateId.JumpAttack);
                return true;
            }

            return false;
        }

        public override void Update()
        {
            base.Update();

            Animation.SetFloat(CommonAnimatorParameters.VelocityY, Movement.VelocityY);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsMovementEnabled && Input.Move.x != 0f)
            {
                Movement.SetVelocityX(
                    Config.Movement.MoveSpeed * Config.Movement.InAirMoveMultiplier * Input.Move.x,
                    true);
            }
        }

        protected void SetMovementEnabled(bool enabled)
        {
            IsMovementEnabled = enabled;
        }
    }
}
