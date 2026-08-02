using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Enemy.States
{
    public class EnemyHurtState : HurtState<EnemyController>
    {
        public override int Id => (int)EnemyStateId.Hurt;

        public EnemyHurtState(EnemyController controller)
            : base(controller, AnimatorHashProvider.Hurt, AnimatorParameterType.Bool)
        {
        }

        protected override void OnHurtFinished()
        {
            if (Controller.TargetDetector.HasTarget)
            {
                Controller.ChangeState(EnemyStateId.Chase);
                return;
            }

            Controller.ChangeState(EnemyStateId.Patrol);
        }
    }
}
