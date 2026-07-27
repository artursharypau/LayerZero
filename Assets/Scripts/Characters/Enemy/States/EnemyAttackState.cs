using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Enemy.States
{
    public class EnemyAttackState : AttackState<EnemyController>
    {
        public EnemyAttackState(EnemyController controller)
            : base(controller, AnimatorHashProvider.Attack)
        {
        }

        protected override void OnAttackFinished()
        {
            Controller.ChangeState(StateId.Chase);
        }
    }
}
