using UnityEngine;

namespace LayerZero.Combat.Damage
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageSource Source;
        public readonly Transform Attacker;
        public readonly DamageImpactInfo Impact;

        public DamageInfo(int amount, DamageSource source, Transform attacker, DamageImpactInfo impact = default)
        {
            Amount = amount;
            Source = source;
            Attacker = attacker;
            Impact = impact;
        }

        public DamageInfo WithImpact(DamageImpactInfo impact)
        {
            return new DamageInfo(Amount, Source, Attacker, impact);
        }

        public static DamageInfo FromDefinition(DamageDefinition definition, Transform attacker)
        {
            DamageImpactInfo impact = DamageImpactInfo.None;

            if (definition.HasImpact && attacker)
            {
                Vector2 knockback = attacker.TransformDirection(definition.Knockback);
                impact = new DamageImpactInfo(knockback, definition.StunDuration);
            }

            return new DamageInfo(definition.Amount, definition.Source, attacker, impact);
        }
    }
}
