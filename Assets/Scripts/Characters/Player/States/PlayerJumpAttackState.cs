using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Animation;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerJumpAttackState : AttackStateBase<PlayerController>
    {
        private bool _hasLanded;

        public PlayerJumpAttackState(PlayerController owner)
            : base(owner, PlayerAnimatorParameters.JumpAttack)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _hasLanded = false;

            Movement.SetVelocity(
                Owner.Config.JumpAttack.Velocity.x * Movement.FacingDirection,
                Owner.Config.JumpAttack.Velocity.y);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_hasLanded || !Movement.IsGrounded)
            {
                return;
            }

            _hasLanded = true;

            Animation.Fire(PlayerAnimatorParameters.JumpAttackLanding);
            Movement.SetVelocityX(0f);
        }

        protected override AttackDefinition ResolveAttack()
        {
            return Owner.Config.JumpAttack.Attack;
        }

        protected override void OnAttackFinished()
        {
            if (Owner.Input.Move.x != 0f)
            {
                ChangeTo<PlayerMoveState>();
            }
            else
            {
                ChangeTo<PlayerIdleState>();
            }
        }
    }
}
