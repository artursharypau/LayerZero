using LayerZero.Core.Randomness;
using LayerZero.Gameplay.Stats;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal sealed class DamageResolver : IDamageResolver
    {
        private const float PercentScale = 100f;

        private readonly IStatsSystem _statsSystem;

        public DamageResolver(IStatsSystem statsSystem)
        {
            _statsSystem = statsSystem;
        }

        public DamageInfo Resolve(DamageDefinition definition, Transform attackerTransform)
        {
            DamageImpactInfo impact = DamageImpactInfo.None;

            if (definition.HasImpact && attackerTransform)
            {
                Vector2 knockback = attackerTransform.TransformDirection(definition.Knockback);
                impact = new DamageImpactInfo(knockback, definition.StunDuration);
            }

            float criticalDamageChance = _statsSystem.Get(StatId.CriticalDamageChance);
            bool isCritical = Chance.Roll(criticalDamageChance);

            float damage = _statsSystem.Get(StatId.Damage);

            if (isCritical)
            {
                float criticalDamageBonus = _statsSystem.Get(StatId.CriticalDamageBonus);
                damage *= 1f + criticalDamageBonus / PercentScale;
            }

            return new DamageInfo(isCritical, damage * definition.Multiplier, definition.Source, attackerTransform, impact);
        }
    }
}
