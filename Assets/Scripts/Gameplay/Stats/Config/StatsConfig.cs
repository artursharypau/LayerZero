using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [CreateAssetMenu(fileName = "StatsConfig", menuName = "LayerZero/Characters/Stats Config")]
    internal sealed class StatsConfig : ScriptableObject
    {
        [SerializeField] private Stat _maxHealth = new();

        [SerializeField] private MajorStats _majorStats = new();
        [SerializeField] private OffenseStats _offenseStats = new();
        [SerializeField] private DefenseStats _defenseStats = new();

        public void CopyTo(IDictionary<StatId, float> values)
        {
            values[StatId.MaxHealth] = _maxHealth.Value;

            _majorStats.CopyTo(values);
            _offenseStats.CopyTo(values);
            _defenseStats.CopyTo(values);
        }
    }
}
