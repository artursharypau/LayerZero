using LayerZero.Characters.Common.Animation;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Common.States
{
    public abstract class AttackStateBase<TCharacter> : CharacterState<TCharacter>
        where TCharacter : Character
    {
        protected AttackStateBase(TCharacter owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public override void Enter()
        {
            OnPrepareAttack();

            base.Enter();

            AttackDefinition attack = ResolveAttack();
            if (attack != null && Owner.Combat)
            {
                Owner.Combat.Arm(attack);
            }

            Animation.AttackFinished += HandleAttackFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Animation.AttackFinished -= HandleAttackFinished;
        }

        protected abstract AttackDefinition ResolveAttack();

        protected virtual void OnPrepareAttack()
        {
        }

        protected abstract void OnAttackFinished();

        private void HandleAttackFinished()
        {
            Animation.AttackFinished -= HandleAttackFinished;
            OnAttackFinished();
        }
    }
}
