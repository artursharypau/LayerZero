using Common;
using Common.Animations;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyState : StateBase
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

        public override void Enter()
        {
            Debug.unityLogger.Log($"Entering in {_counter++}: {GetType().Name}");

            base.Enter();
        }

        public override void Exit()
        {
            Debug.unityLogger.Log($"Exiting in {_counter++}: {GetType().Name}");

            base.Exit();
        }
    }
}
