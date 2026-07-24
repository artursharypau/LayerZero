using Characters.Common;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

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
            FSM.ChangeState(Controller.ChaseState);
        }
    }
}
