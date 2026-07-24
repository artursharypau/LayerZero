using Characters.Common;
using Infrastructure.Animation;

namespace Characters.Enemy.States
{
    public abstract class EnemyState : CharacterState<EnemyController>
    {
        protected EnemyState(EnemyController controller, int hash, AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(controller, hash, type)
        {
        }
    }
}
