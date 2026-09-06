using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats
{
    internal static class StatFormulas
    {
        private const float HealthPerVitality = 5f;

        private const float DamagePerStrength = 1f;
        private const float CriticalDamageBonusPerStrength = 0.5f;

        private const float CriticalDamageChancePerAgility = 0.5f;
        private const float EvasionChancePerAgility = 0.5f;

        private const float MaxCriticalDamageChance = 50f;
        private const float MaxEvasionChance = 30f;

        public static void ApplyDerived(IDictionary<StatId, float> values)
        {
            float strength = values[StatId.Strength];
            float agility = values[StatId.Agility];
            float vitality = values[StatId.Vitality];

            values[StatId.MaxHealth] += vitality * HealthPerVitality;

            values[StatId.Damage] += strength * DamagePerStrength;
            values[StatId.CriticalDamageBonus] += strength * CriticalDamageBonusPerStrength;

            float criticalDamageChance = values[StatId.CriticalDamageChance] + agility * CriticalDamageChancePerAgility;
            values[StatId.CriticalDamageChance] = Mathf.Clamp(criticalDamageChance, 0f, MaxCriticalDamageChance);

            float evasionChance = values[StatId.EvasionChance] + agility * EvasionChancePerAgility;
            values[StatId.EvasionChance] = Mathf.Clamp(evasionChance, 0f, MaxEvasionChance);
        }
    }
}
