using LayerZero.Core.Randomness;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Stats;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal sealed class DamageResolver : IDamageResolver
    {
        private readonly IStatsSystem _statsSystem;
        private readonly IElementalAffinity _elementalAffinity;

        public DamageResolver(IStatsSystem statsSystem, IElementalAffinity elementalAffinity)
        {
            _statsSystem = statsSystem;
            _elementalAffinity = elementalAffinity;
        }

        public DamagePayload Resolve(DamageDefinition definition, Transform attackerTransform)
        {
            DamageImpactInfo impact = DamageImpactInfo.None;

            if (definition.HasImpact && attackerTransform)
            {
                Vector2 knockback = attackerTransform.TransformDirection(definition.Knockback);
                impact = new DamageImpactInfo(knockback, definition.StunDuration);
            }

            float criticalDamageChance = _statsSystem.Get(StatId.CriticalDamageChance);
            bool isCritical = Chance.Roll(criticalDamageChance);

            float damage = isCritical
                ? _statsSystem.Get(StatId.CriticalDamage)
                : _statsSystem.Get(StatId.Damage);

            DamageInfo damageInfo = new(isCritical, damage * definition.Multiplier, definition.Source, attackerTransform, impact);
            ElementalEffectInfo elementalEffectInfo = default;

            ElementKind elementKind = _elementalAffinity.Kind;

            if (definition.CanApplyElementalEffect && elementKind != ElementKind.None)
            {
                float elementalDamageChance = _statsSystem.Get(StatId.ElementalDamageChance);
                if (Chance.Roll(elementalDamageChance))
                {
                    float elementalDamage = _statsSystem.Get(StatId.ElementalDamage);
                    float elementalDamageDuration = _statsSystem.Get(StatId.ElementalDamageDuration);

                    elementalEffectInfo = new ElementalEffectInfo(elementKind, elementalDamage, elementalDamageDuration);
                }
            }

            return new DamagePayload(damageInfo, elementalEffectInfo);
        }
    }
}
