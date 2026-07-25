using UnityEngine;

namespace Systems.Damage
{
    public readonly struct DamageImpactInfo
    {
        public static readonly DamageImpactInfo None = new();

        public readonly Vector2 Knockback;
        public readonly float StunDuration;

        public DamageImpactInfo(Vector2 knockback, float stunDuration = 0f)
        {
            Knockback = knockback;
            StunDuration = stunDuration;
        }

        public static bool operator ==(DamageImpactInfo left, DamageImpactInfo right)
        {
            return left.Knockback == right.Knockback && Mathf.Approximately(left.StunDuration, right.StunDuration);
        }

        public static bool operator !=(DamageImpactInfo left, DamageImpactInfo right)
        {
            return !(left == right);
        }
    }
}
