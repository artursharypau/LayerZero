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

        public override void Enter()
        {
            base.Enter();

            Controller.Combat.SetActiveAttackDefinition(Controller.AttackDefinition);
        }

        protected override void OnAttackFinished()
        {
            Controller.ChangeState(EnemyStateId.Chase);
        }
    }
}
