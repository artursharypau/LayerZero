using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageSource Source;
        public readonly Transform AttackerTransform;
        public readonly DamageImpactInfo Impact;

        public DamageInfo(int amount, DamageSource source, Transform attackerTransform, DamageImpactInfo impact = default)
        {
            Amount = amount;
            Source = source;
            AttackerTransform = attackerTransform;
            Impact = impact;
        }

        public DamageInfo WithImpact(DamageImpactInfo impact)
        {
            return new DamageInfo(Amount, Source, AttackerTransform, impact);
        }

        public static DamageInfo FromDefinition(DamageDefinition definition, Transform attackerTransform)
        {
            DamageImpactInfo impact = DamageImpactInfo.None;

            if (definition.HasImpact && attackerTransform)
            {
                Vector2 knockback = attackerTransform.TransformDirection(definition.Knockback);
                impact = new DamageImpactInfo(knockback, definition.StunDuration);
            }

            return new DamageInfo(definition.Amount, definition.Source, attackerTransform, impact);
        }
    }
}
