using Common;
using Common.Animations;

namespace Enemy
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
            : base(fsm, new AnimationContext(animParameterHash, animParameterType, controller.Anim))
        {
            Controller = controller;
        }
    }
}
