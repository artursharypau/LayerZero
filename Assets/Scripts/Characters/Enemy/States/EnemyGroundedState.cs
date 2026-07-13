using Core.Animation;
using Core.StateMachine;

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

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.Target.HasCurrent)
            {
                FSM.ChangeState(Controller.ChaseState);
                return true;
            }

            return false;
        }
    }
}
