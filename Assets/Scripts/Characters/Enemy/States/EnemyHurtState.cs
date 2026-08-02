using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Enemy.States
{
    public class EnemyHurtState : HurtState<EnemyController>
    {
        public EnemyHurtState(EnemyController controller)
            : base(controller, AnimatorHashProvider.Hurt, AnimatorParameterType.Bool)
        {
        }

        protected override void OnHurtFinished()
        {
            if (Controller.TargetDetector.Target)
            {
                Controller.ChangeState(EnemyStateId.Chase);
                return;
            }

            Controller.ChangeState(EnemyStateId.Idle);
        }
    }
}
