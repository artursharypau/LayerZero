using System.Collections.Generic;
using LayerZero.Gameplay.Stats.Config;

namespace LayerZero.Gameplay.Stats
{
    internal sealed class StatsSystem : IStatsSystem
    {
        private readonly Dictionary<StatId, Stat> _values = new();

        public StatsSystem(StatsConfig config)
        {
            StatsBuilder.Build(config, _values);
        }

        public float Get(StatId id)
        {
            return _values.TryGetValue(id, out Stat stat) ? stat.Value : 0f;
        }
    }
}
