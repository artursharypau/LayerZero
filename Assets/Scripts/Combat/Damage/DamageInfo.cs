using UnityEngine;

namespace LayerZero.Combat.Damage
{
    /// <summary>A hit in flight: authoring data resolved against a concrete attacker and impact space.</summary>
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageSource Source;

        /// <summary>Who is responsible for the hit (used for aggro), not necessarily what touched the victim.</summary>
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

        /// <param name="impactSpace">
        /// Transform whose orientation defines the knockback direction. Differs from
        /// <paramref name="attacker" /> for projectiles, where knockback follows the flight direction.
        /// </param>
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
