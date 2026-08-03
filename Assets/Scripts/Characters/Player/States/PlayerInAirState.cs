using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    /// <summary>Shared airborne behaviour: reduced air control and the dive attack.</summary>
    public abstract class PlayerInAirState : PlayerState
    {
        private bool _isMovementEnabled = true;

        protected PlayerInAirState(PlayerController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        protected bool IsMovementEnabled => _isMovementEnabled;

        public override void Enter()
        {
            base.Enter();

            _isMovementEnabled = true;
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (_isMovementEnabled && Input.WasPerformed(PlayerInputAction.Attack))
            {
                ChangeTo<PlayerJumpAttackState>();
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

            if (_isMovementEnabled && Input.Move.x != 0f)
            {
                Movement.SetVelocityX(
                    Config.Movement.MoveSpeed * Config.Movement.InAirMoveMultiplier * Input.Move.x,
                    true);
            }
        }

        protected void SetMovementEnabled(bool enabled)
        {
            _isMovementEnabled = enabled;
        }
    }
}
