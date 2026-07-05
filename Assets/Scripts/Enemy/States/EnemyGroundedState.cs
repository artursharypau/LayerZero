using Common;
using Common.Animations;

namespace Enemy.States
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

        public override void Update()
        {
            if (Controller.IsPlayerDetected)
            {
                FSM.ChangeState(Controller.BattleState);
            }

            base.Update();
        }
    }
}
