using Common;
using Common.Animations;

namespace Enemy.States
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(StateMachine fsm, EnemyController controller)
            : base(fsm, controller, AnimationHashProvider.Attack, AnimatorParameterType.Trigger)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.AnimTriggers.AttackFinished += OnFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AnimTriggers.AttackFinished -= OnFinished;
        }

        private void OnFinished()
        {
            Controller.AnimTriggers.AttackFinished -= OnFinished;

            FSM.ChangeState(Controller.BattleState);
        }
    }
}
