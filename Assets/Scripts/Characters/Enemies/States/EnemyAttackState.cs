using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Combat.Attacks;

namespace LayerZero.Characters.Enemies.States
{
    public abstract class EnemyAttackState : AttackStateBase<EnemyController>
    {
        protected EnemyAttackState(EnemyController owner)
            : base(owner, CommonAnimatorParameters.Attack)
        {
        }

        protected override AttackDefinition ResolveAttackDefinition()
        {
            return Owner.Config.Attack;
        }

        protected override void OnAttackFinished()
        {
            ChangeTo(EnemyStateId.Chase);
        }
    }
}
