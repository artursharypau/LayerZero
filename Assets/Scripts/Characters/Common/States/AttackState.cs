using Characters.Common.Animation;

namespace Characters.Common.States
{
    public abstract class AttackState<TController> : AnimatedState<TController>
        where TController : CharacterController2D
    {
        protected AttackState(TController controller, int animHash, AnimatorParameterType type = AnimatorParameterType.Trigger)
            : base(controller, animHash, type)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Controller.AttackAnimatorEvents.AttackFinished += HandleAttackFinished;
        }

        public override void Exit()
        {
            base.Exit();

            Controller.AttackAnimatorEvents.AttackFinished -= HandleAttackFinished;
        }

        protected abstract void OnAttackFinished();

        private void HandleAttackFinished()
        {
            Controller.AttackAnimatorEvents.AttackFinished -= HandleAttackFinished;
            OnAttackFinished();
        }
    }
}
