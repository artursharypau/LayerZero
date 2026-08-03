using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>
    /// Shared enemy attack flow. Concrete archetypes only decide what happens around the swing,
    /// so an archer is a subclass, not a branch inside a shared state.
    /// </summary>
    public abstract class EnemyAttackState : AttackStateBase<EnemyController>
    {
        protected EnemyAttackState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Attack)
        {
        }

        protected override AttackDefinition ResolveAttack()
        {
            return Owner.Config.Attack;
        }

        protected override void OnAttackFinished()
        {
            ChangeTo<EnemyChaseState>();
        }
    }
}
