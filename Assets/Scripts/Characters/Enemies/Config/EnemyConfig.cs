using LayerZero.Combat.Attack;
using UnityEngine;

namespace LayerZero.Characters.Enemies.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "LayerZero/Characters/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyMovementConfig _movement = new();
        [SerializeField] private EnemyChaseConfig _chase = new();
        [SerializeField] private PerceptionConfig _perception = new();
        [SerializeField] private AttackDefinition _attack = new();

        public EnemyMovementConfig Movement => _movement;
        public EnemyChaseConfig Chase => _chase;
        public PerceptionConfig Perception => _perception;
        public AttackDefinition Attack => _attack;
    }
}
