using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Enemies.Config;
using LayerZero.Characters.Enemies.Perception;

namespace LayerZero.Characters.Enemies.States
{
    /// <summary>Base for enemy states. Exposes the config and the senses every enemy state reads.</summary>
    public abstract class EnemyState : CharacterState<EnemyController>
    {
        protected EnemyState(EnemyController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        protected EnemyConfig Config => Owner.Config;
        protected TargetPerception Perception => Owner.Perception;
    }
}
