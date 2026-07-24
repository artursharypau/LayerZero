using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Enemy.States
{
    public abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(
            StateMachine fsm,
            EnemyController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, controller, animParameterHash, animParameterType)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.TargetDetector.TargetFound += OnTargetFound;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.TargetDetector.TargetFound -= OnTargetFound;
        }

        private void OnTargetFound()
        {
            Controller.TargetDetector.TargetFound -= OnTargetFound;
            FSM.ChangeState(Controller.ChaseState);
        }
    }
}
