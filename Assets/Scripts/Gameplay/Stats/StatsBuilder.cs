using System.Collections.Generic;
using LayerZero.Gameplay.Stats.Config;
using UnityEngine;

namespace LayerZero.Gameplay.Stats
{
    internal static class StatsBuilder
    {
        private const float PercentScale = 100f;

        private const float HealthPerVitality = 5f;

        private const float DamagePerStrength = 1f;
        private const float CriticalDamagePerStrength = 0.5f;
        private const float CriticalDamageChancePerAgility = 0.5f;
        private const float MaxCriticalDamageChance = 0.5f;

        private const float ElementalDamagePerIntelligence = 0.5f;
        private const float ElementalDamageChancePerAgility = 0.5f;
        private const float MaxElementalDamageChance = 1f;

        private const float ArmorPerVitality = 1f;
        private const float ArmorMitigationScaling = 100f;
        private const float MaxArmorMitigation = 0.85f;

        private const float EvasionChancePerAgility = 0.5f;
        private const float MaxEvasionChance = 0.3f;

        private const float MaxElementalResistance = 0.75f;

        public static void Build(StatsConfig config, IDictionary<StatId, Stat> values)
        {
            MajorStats major = config.Major;

            Set(values, StatId.MaxHealth, config.MaxHealth + major.Vitality * HealthPerVitality);

            BuildMajorStats(major, values);
            BuildOffenseStats(config.Offense, major, values);
            BuildDefenseStats(config.Defense, major, values);
        }

        private static void BuildMajorStats(MajorStats major, IDictionary<StatId, Stat> values)
        {
            Set(values, StatId.Strength, major.Strength);
            Set(values, StatId.Agility, major.Agility);
            Set(values, StatId.Intelligence, major.Intelligence);
            Set(values, StatId.Vitality, major.Vitality);
        }

        private static void BuildOffenseStats(OffenseStats offense, MajorStats major, IDictionary<StatId, Stat> values)
        {
            float strength = major.Strength;
            float agility = major.Agility;
            float intelligence = major.Intelligence;

            float damage = offense.Damage + strength * DamagePerStrength;
            Set(values, StatId.Damage, damage);

            float criticalDamageBonus = (offense.CriticalDamageBonus + strength * CriticalDamagePerStrength) / PercentScale;
            Set(values, StatId.CriticalDamage, damage * (1f + criticalDamageBonus));

            float criticalDamageChance = (offense.CriticalDamageChance + agility * CriticalDamageChancePerAgility) / PercentScale;
            Set(values, StatId.CriticalDamageChance, Mathf.Clamp(criticalDamageChance, 0f, MaxCriticalDamageChance));

            Set(values, StatId.ElementalDamage, offense.ElementalDamage + intelligence * ElementalDamagePerIntelligence);

            float elementalDamageChance = (offense.ElementalDamageChance + agility * ElementalDamageChancePerAgility) / PercentScale;
            Set(values, StatId.ElementalDamageChance, Mathf.Clamp(elementalDamageChance, 0f, MaxElementalDamageChance));

            Set(values, StatId.ElementalDamageDuration, offense.ElementalDamageDuration);
        }

        private static void BuildDefenseStats(DefenseStats defense, MajorStats major, IDictionary<StatId, Stat> values)
        {
            float vitality = major.Vitality;
            float agility = major.Agility;

            float armor = defense.Armor + vitality * ArmorPerVitality;
            Set(values, StatId.Armor, armor);

            float armorMitigation = armor / (armor + ArmorMitigationScaling);
            Set(values, StatId.ArmorMitigation, Mathf.Clamp(armorMitigation, 0f, MaxArmorMitigation));

            float evasionChance = (defense.EvasionChance + agility * EvasionChancePerAgility) / PercentScale;
            Set(values, StatId.EvasionChance, Mathf.Clamp(evasionChance, 0f, MaxEvasionChance));

            float elementalResistance = defense.ElementalResistance / PercentScale;
            Set(values, StatId.ElementalResistance, Mathf.Clamp(elementalResistance, 0f, MaxElementalResistance));
        }

        private static void Set(IDictionary<StatId, Stat> values, StatId id, float value)
        {
            if (!values.TryGetValue(id, out Stat stat))
            {
                stat = new Stat();
                values.Add(id, stat);
            }

            stat.Value = value;
        }
    }
}
