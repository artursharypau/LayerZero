using System.Collections.Generic;
using LayerZero.Gameplay.Stats.Config;
using UnityEngine;

namespace LayerZero.Gameplay.Stats
{
    internal sealed class StatsSystem : MonoBehaviour, IStatsSystem
    {
        [SerializeField] private StatsConfig _stats;

        private readonly Dictionary<StatId, float> _values = new();

        private bool _isBuilt;

        public float Get(StatId id)
        {
            EnsureBuilt();
            return _values.GetValueOrDefault(id);
        }

        private void EnsureBuilt()
        {
            if (_isBuilt)
            {
                return;
            }

            _isBuilt = true;
            Rebuild();
        }

        private void Rebuild()
        {
            _stats.CopyTo(_values);
            StatFormulas.ApplyDerived(_values);
        }
    }
}
