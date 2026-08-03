using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    /// <summary>
    /// Tuning shared by every enemy archetype. New archetypes derive from this asset type and
    /// add their own section (see <see cref="RangedEnemyConfig" />) instead of growing this one.
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "LayerZero/Characters/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyMovementSettings _movement = new();
        [SerializeField] private EnemyChaseSettings _chase = new();
        [SerializeField] private PerceptionSettings _perception = new();
        [SerializeField] private AttackDefinition _attack = new();

        public EnemyMovementSettings Movement => _movement;
        public EnemyChaseSettings Chase => _chase;
        public PerceptionSettings Perception => _perception;
        public AttackDefinition Attack => _attack;
    }
}
