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
            return FromDefinition(definition, attacker, attacker);
        }

        public static DamageInfo FromDefinition(DamageDefinition definition, Transform attacker, Transform impactSpace)
        {
            DamageImpactInfo impact = DamageImpactInfo.None;

            if (definition.HasImpact && impactSpace)
            {
                Vector2 knockback = impactSpace.TransformDirection(definition.Knockback);
                impact = new DamageImpactInfo(knockback, definition.StunDuration);
            }

            return new DamageInfo(definition.Amount, definition.Source, attacker, impact);
        }
    }
}
