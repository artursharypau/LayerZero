using LayerZero.Characters.Common.Animation;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Common.States
{
    /// <summary>
    /// Shared attack flow: arm the combat system, wait for the animation to report that the
    /// attack is over, then decide where to go. Melee and ranged attacks use the same flow -
    /// only <see cref="ResolveAttack" /> differs.
    /// </summary>
    public abstract class AttackStateBase<TCharacter> : CharacterState<TCharacter>
        where TCharacter : Character
    {
        protected AttackStateBase(TCharacter owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        public override void Enter()
        {
            // Runs before the animation parameter so states can pick a combo index first.
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

        /// <summary>The attack about to be performed. Read once, on enter.</summary>
        protected abstract AttackDefinition ResolveAttack();

        /// <summary>Called before the animation is triggered - set combo indices and similar here.</summary>
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
