using UnityEngine;

namespace LayerZero.Combat.Damage.Resistance
{
    public readonly struct DamageResistance
    {
        public static readonly DamageResistance Default = new(false, false, 1f);

        public readonly bool IsInvulnerable;
        public readonly bool IgnoresStun;
        public readonly float KnockbackMultiplier;

        private DamageResistance(bool isInvulnerable, bool ignoresStun, float knockbackMultiplier)
        {
            IsInvulnerable = isInvulnerable;
            IgnoresStun = ignoresStun;
            KnockbackMultiplier = knockbackMultiplier;
        }

        public DamageResistance WithInvulnerability()
        {
            return new DamageResistance(true, IgnoresStun, KnockbackMultiplier);
        }

        public DamageResistance WithStunImmunity()
        {
            return new DamageResistance(IsInvulnerable, true, KnockbackMultiplier);
        }

        public DamageResistance WithKnockbackMultiplier(float multiplier)
        {
            return new DamageResistance(IsInvulnerable, IgnoresStun, Mathf.Max(0f, multiplier));
        }
    }
}
