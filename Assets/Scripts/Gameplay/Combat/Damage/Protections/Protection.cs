using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage.Protections
{
    internal readonly struct Protection
    {
        public static readonly Protection Invulnerability = new(ProtectionKind.Invulnerability);
        public static readonly Protection StunImmunity = new(ProtectionKind.StunImmunity);

        public readonly ProtectionKind Kind;
        public readonly float Value;

        private Protection(ProtectionKind kind, float value = 0f)
        {
            Kind = kind;
            Value = value;
        }

        public static Protection Knockback(float multiplier)
        {
            return new Protection(ProtectionKind.Knockback, Mathf.Clamp01(multiplier));
        }
    }
}
