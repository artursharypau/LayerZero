using System;
using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal class MajorStats
    {
        [SerializeField] private Stat _strength;
        [SerializeField] private Stat _agility;
        [SerializeField] private Stat _intelligence;
        [SerializeField] private Stat _vitality;

        public void CopyTo(IDictionary<StatId, float> values)
        {
            values[StatId.Strength] = _strength.Value;
            values[StatId.Agility] = _agility.Value;
            values[StatId.Intelligence] = _intelligence.Value;
            values[StatId.Vitality] = _vitality.Value;
        }
    }
}
