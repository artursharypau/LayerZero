using Core.Animation;
using Core.StateMachine;

namespace Characters.Common
{
    public abstract class AttackState<TController> : State
        where TController : CharacterController
    {
        protected TController Controller { get; }

        protected AttackState(
            StateMachine fsm,
            TController controller,
            int animHash,
            AnimatorParameterType type = AnimatorParameterType.Trigger)
            : base(fsm, new AnimatorContext(animHash, type, controller.Anim))
        {
            Controller = controller;
        }

        public override void Enter()
        {
            base.Enter();

            Controller.AnimTriggers.AttackFinished += OnAttackFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AnimTriggers.AttackFinished -= OnAttackFinished;
        }

        protected abstract void OnAttackFinished();
    }
}
