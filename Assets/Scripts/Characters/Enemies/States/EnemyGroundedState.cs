using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Enemies.States
{
    public abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(EnemyController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
            OnFixed(() => Perception.HasTarget, EnemyStateId.Chase);
        }
    }
}
