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

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.IsPlayerDetected)
            {
                FSM.ChangeState(Controller.BattleState);
                return true;
            }

            return false;
        }
    }
}
