using Core.Animation;
using Core.StateMachine;

namespace Characters.Enemy.States
{
    public abstract class EnemyState : State
    {
        private static int _counter;

        protected EnemyController Controller { get; }

        protected EnemyState(
            StateMachine fsm,
            EnemyController controller,
            int animParameterHash,
            AnimatorParameterType animParameterType = AnimatorParameterType.Bool)
            : base(fsm, new AnimatorContext(animParameterHash, animParameterType, controller.Anim))
        {
            Controller = controller;
        }
    }
}
