using System;
using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal class DefenseStats
    {
        [SerializeField] private Stat _armor;
        [SerializeField] private Stat _evasion;

        [SerializeField] private Stat _fireResistance;
        [SerializeField] private Stat _iceResistance;
        [SerializeField] private Stat _lightResistance;

        public void CopyTo(IDictionary<StatId, float> values)
        {
            values[StatId.Armor] = _armor.Value;
            values[StatId.Evasion] = _evasion.Value;

            values[StatId.FireResistance] = _fireResistance.Value;
            values[StatId.IceResistance] = _iceResistance.Value;
            values[StatId.LightResistance] = _lightResistance.Value;
        }
    }
}
