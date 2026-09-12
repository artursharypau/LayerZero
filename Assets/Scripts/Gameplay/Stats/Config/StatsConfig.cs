using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [CreateAssetMenu(fileName = "StatsConfig", menuName = "LayerZero/Characters/Stats Config")]
    internal sealed class StatsConfig : ScriptableObject
    {
        [SerializeField] private float _maxHealth;

        [SerializeField] private MajorStats _majorStats = new();
        [SerializeField] private OffenseStats _offenseStats = new();
        [SerializeField] private DefenseStats _defenseStats = new();

        public float MaxHealth => _maxHealth;

        public MajorStats Major => _majorStats;
        public OffenseStats Offense => _offenseStats;
        public DefenseStats Defense => _defenseStats;
    }
}
