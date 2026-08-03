using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
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
