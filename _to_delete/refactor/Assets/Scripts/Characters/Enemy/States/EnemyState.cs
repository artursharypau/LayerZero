using Characters.Common.Animation;
using Characters.Common.States;

namespace Characters.Enemy.States
{
    public abstract class EnemyState : AnimatedState<EnemyController>
    {
        protected EnemyState(EnemyController controller, int hash, AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(controller, hash, type)
        {
        }
    }
}
