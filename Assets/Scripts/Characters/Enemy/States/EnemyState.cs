using Characters.Common;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Enemy.States
{
    public abstract class EnemyState : CharacterState<EnemyController>
    {
        protected EnemyState(
            StateMachine fsm,
            EnemyController controller,
            int hash,
            AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(fsm, controller, hash, type)
        {
        }
    }
}
