using LayerZero.Gameplay.Characters.Common.States;
using LayerZero.Gameplay.Characters.Enemies.Config;
using LayerZero.Gameplay.Characters.Enemies.Perception;

namespace LayerZero.Gameplay.Characters.Enemies.States
{
    public abstract class EnemyState : CharacterState<EnemyController>
    {
        protected EnemyState(EnemyController owner)
            : base(owner)
        {
        }

        protected EnemyConfig Config => Owner.Config;
        protected EnemyTargetPerception Perception => Owner.Perception;
    }
}
