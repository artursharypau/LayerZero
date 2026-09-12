using LayerZero.Gameplay.Combat.Attack;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Stats.Config;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Enemies.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "LayerZero/Characters/Enemy Config")]
    internal sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField] private StatsConfig _stats;

        [SerializeField] private EnemyMovementConfig _movement = new();
        [SerializeField] private EnemyChaseConfig _chase = new();
        [SerializeField] private EnemyPerceptionConfig _perception = new();
        [SerializeField] private AttackDefinition _attack = new();
        [SerializeField] private ElementKind _initialElement = ElementKind.None;

        public StatsConfig Stats => _stats;

        public EnemyMovementConfig Movement => _movement;
        public EnemyChaseConfig Chase => _chase;
        public EnemyPerceptionConfig Perception => _perception;
        public AttackDefinition Attack => _attack;
        public ElementKind InitialElement => _initialElement;
    }
}
