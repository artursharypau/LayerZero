using Characters.Common;
using Core.Animation;
using Core.StateMachine;

namespace Characters.Enemy.States
{
    public class EnemyAttackState : AttackState<EnemyController>
    {
        public EnemyAttackState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, AnimatorHashProvider.Attack)
        {
        }

        protected override void OnAttackFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnAttackFinished;

            FSM.ChangeState(Controller.ChaseState);
        }
    }
}
