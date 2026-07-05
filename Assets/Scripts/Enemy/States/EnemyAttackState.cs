using Common;
using Common.Animations;

namespace Enemy.States
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(StateMachine fsm, EnemyController controller)
            : base(AnimationIdProvider.Attack, fsm, controller)
        {
        }

        public override void Enter()
        {
            Controller.AnimTriggers.AttackFinished += OnFinished;

            base.Enter();
        }

        public override void Exit()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            base.Exit();
        }

        private void OnFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            FSM.ChangeState(Controller.BattleState);
        }
    }
}
