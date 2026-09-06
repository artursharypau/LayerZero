using System;
using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats.Config
{
    [Serializable]
    internal class OffenseStats
    {
        [SerializeField] private Stat _damage;
        [SerializeField] private Stat _criticalDamageBonus;
        [SerializeField] private Stat _criticalDamageChance;

        [SerializeField] private Stat _fireDamage;
        [SerializeField] private Stat _iceDamage;
        [SerializeField] private Stat _lightDamage;

        public void CopyTo(IDictionary<StatId, float> values)
        {
            values[StatId.Damage] = _damage.Value;
            values[StatId.CriticalDamageBonus] = _criticalDamageBonus.Value;
            values[StatId.CriticalDamageChance] = _criticalDamageChance.Value;

            values[StatId.FireDamage] = _fireDamage.Value;
            values[StatId.IceDamage] = _iceDamage.Value;
            values[StatId.LightDamage] = _lightDamage.Value;
        }
    }
}
