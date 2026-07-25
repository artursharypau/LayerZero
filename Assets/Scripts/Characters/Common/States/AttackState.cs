using Characters.Common.Animation;
using Core.StateMachine;

namespace Characters.Common.States
{
    public abstract class AttackState<TController> : State
        where TController : CharacterControllerBase
    {
        protected TController Controller { get; }

        protected AttackState(TController controller, int animHash, AnimatorParameterType type = AnimatorParameterType.Trigger)
            : base(new AnimatorContext(animHash, type, controller.Anim))
        {
            Controller = controller;
        }

        public override void Enter()
        {
            base.Enter();

            Controller.AttackFeedback.AttackFinished += HandleAttackFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AttackFeedback.AttackFinished -= HandleAttackFinished;
        }

        protected abstract void OnAttackFinished();

        private void HandleAttackFinished()
        {
            Controller.AttackFeedback.AttackFinished -= HandleAttackFinished;
            OnAttackFinished();
        }
    }
}
