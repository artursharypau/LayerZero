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

        public bool HasImpact => Knockback != Vector2.zero || StunDuration > 0f;
    }
}
