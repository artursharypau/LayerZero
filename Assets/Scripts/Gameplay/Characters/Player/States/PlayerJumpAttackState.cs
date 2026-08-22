using LayerZero.Gameplay.Characters.Common.States;
using LayerZero.Gameplay.Characters.Player.Abilities;
using LayerZero.Gameplay.Characters.Player.Animation;
using LayerZero.Gameplay.Combat.Attack;

namespace LayerZero.Gameplay.Characters.Player.States
{
    public sealed class PlayerJumpAttackState : PlayerState
    {
        private readonly AttackBehaviour _attack;

        private bool _hasLanded;

        public PlayerJumpAttackState(PlayerController owner)
            : base(owner)
        {
            _attack = new AttackBehaviour(owner, ResolveAttack);

            On(() => _attack.IsFinished, ResolveLocomotionState);
            On(() => Owner.Abilities.CanUse(PlayerAbilityId.Dash), PlayerStateId.Dash);
        }

        public override int Id => PlayerStateId.JumpAttack;

        public override void Enter()
        {
            base.Enter();

            _hasLanded = false;

            Movement.SetVelocity(Config.JumpAttack.Velocity.x * Movement.FacingDirection, Config.JumpAttack.Velocity.y);

            _attack.Begin();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_hasLanded || !Movement.IsGrounded)
            {
                return;
            }

            _hasLanded = true;

            Animator.Trigger(PlayerAnimatorParameters.JumpAttackTrigger);
            Movement.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();

            _attack.End();
        }

        private AttackDefinition ResolveAttack()
        {
            return Config.JumpAttack.Attack;
        }
    }
}
