using UnityEngine;

namespace Systems.Damage
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageSource Source;
        public readonly Transform AttackerTransform;
        public readonly DamageImpactInfo Impact;

        private DamageInfo(int amount, DamageSource source, Transform attackerTransform, DamageImpactInfo impact = default)
        {
            Amount = amount;
            Source = source;
            AttackerTransform = attackerTransform;
            Impact = impact;
        }

        public static DamageInfo FromDefinition(DamageDefinition damageDefinition, Transform attackerTransform)
        {
            DamageImpactInfo damageImpact = DamageImpactInfo.None;

            if (damageDefinition.Knockback != Vector2.zero || damageDefinition.StunDuration != 0f)
            {
                Vector2 knockback = attackerTransform.TransformDirection(damageDefinition.Knockback);
                damageImpact = new DamageImpactInfo(knockback, damageDefinition.StunDuration);
            }

            return new DamageInfo(
                damageDefinition.Amount,
                damageDefinition.Source,
                attackerTransform,
                damageImpact);
        }
    }
}
