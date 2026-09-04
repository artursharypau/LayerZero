using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Stats
{
    internal static class StatFormulas
    {
        private const float HealthPerVitality = 2f;
        private const float EvasionPerAgility = 2f;

        private const float MaxEvasion = 50f;

        public static void ApplyDerived(IDictionary<StatId, float> values)
        {
            values[StatId.MaxHealth] += values[StatId.Vitality] * HealthPerVitality;

            float evasion = values[StatId.Evasion] + values[StatId.Agility] * EvasionPerAgility;
            values[StatId.Evasion] += Mathf.Clamp(evasion, 0f, MaxEvasion);
        }
    }
}
