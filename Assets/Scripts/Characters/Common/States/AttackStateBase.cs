using LayerZero.Characters.Common.Animation;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Common.States
{
    public abstract class AttackStateBase<TCharacter> : CharacterState<TCharacter>
        where TCharacter : Character2D
    {
        protected AttackStateBase(TCharacter owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public override void Enter()
        {
            base.Enter();

            OnPrepareAttack();

            AttackDefinition attack = ResolveAttackDefinition();
            if (attack != null)
            {
                Owner.Combat.Arm(attack);
            }

            Animator.Events.AttackFinished += HandleAttackFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Animator.Events.AttackFinished -= HandleAttackFinished;
        }

        protected abstract AttackDefinition ResolveAttackDefinition();
        protected abstract void OnAttackFinished();

        protected virtual void OnPrepareAttack()
        {
        }

        private void HandleAttackFinished()
        {
            Animator.Events.AttackFinished -= HandleAttackFinished;
            OnAttackFinished();
        }
    }
}
