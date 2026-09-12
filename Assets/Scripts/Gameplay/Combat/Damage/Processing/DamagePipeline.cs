using LayerZero.Core.Diagnostics;
using LayerZero.Core.Randomness;
using LayerZero.Gameplay.Combat.Damage.Protections;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Gameplay.Stats;
using LayerZero.Gameplay.StatusEffects;
using LayerZero.Gameplay.StatusEffects.Effects.Damage;
using LayerZero.Gameplay.StatusEffects.Effects.Slowdown;
using UnityEngine;
using VContainer;

namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal sealed class DamagePipeline : MonoBehaviour, IDamagePipeline
    {
        private IDamageReceiver _damageReceiver;
        private IDamageProtection _protection;
        private IStatsSystem _statsSystem;
        private IStatusEffectsSystem _statusEffectsSystem;

        [Inject]
        public void Construct(
            IDamageReceiver damageReceiver,
            IDamageProtection protection,
            IStatsSystem statsSystem,
            IStatusEffectsSystem statusEffectsSystem)
        {
            _damageReceiver = damageReceiver;
            _protection = protection;
            _statsSystem = statsSystem;
            _statusEffectsSystem = statusEffectsSystem;
        }

        public bool Process(in DamagePayload payload)
        {
            if (_damageReceiver.IsDead || _protection.IsInvulnerable)
            {
                return false;
            }

            float evasionChance = _statsSystem.Get(StatId.EvasionChance);
            if (Chance.Roll(evasionChance))
            {
                return false;
            }

            DamageInfo damageInfo = payload.Damage;

            float armorMitigation = _statsSystem.Get(StatId.ArmorMitigation);
            float resolvedDamage = damageInfo.Amount * (1f - armorMitigation);
            DamageImpactInfo resolvedImpact = _protection.Resolve(damageInfo.Impact);

            DamageInfo resolved = new(
                damageInfo.IsCritical,
                resolvedDamage,
                damageInfo.Source,
                damageInfo.AttackerTransform,
                resolvedImpact);

            _damageReceiver.TakeDamage(resolved);

            if (payload.HasElementalEffect)
            {
                ApplyElementalEffect(payload.ElementalEffect);
            }

            return true;
        }

        private void ApplyElementalEffect(ElementalEffectInfo elementalEffect)
        {
            float elementalResistance = _statsSystem.Get(StatId.ElementalResistance);
            float resolvedAmount = elementalEffect.Value * (1f - elementalResistance);

            switch (elementalEffect.Kind)
            {
                case ElementKind.None:
                    break;
                case ElementKind.Fire:
                    _statusEffectsSystem.Apply(new DamageOverTimeEffect(resolvedAmount, elementalEffect.Duration));
                    break;
                case ElementKind.Ice:
                    _statusEffectsSystem.Apply(new SlowdownOverTimeEffect(resolvedAmount, elementalEffect.Duration));
                    break;
                default:
                    GameLog.Error(this, $"'{name}' has no status effect for element '{elementalEffect.Kind}'.");
                    break;
            }
        }
    }
}
