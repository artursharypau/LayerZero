using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal readonly struct DamageInfo
    {
        public readonly bool IsCritical;
        public readonly float Amount;
        public readonly DamageSource Source;
        public readonly Transform AttackerTransform;
        public readonly DamageImpactInfo Impact;

        public DamageInfo(
            bool isCritical,
            float amount,
            DamageSource source,
            Transform attackerTransform,
            DamageImpactInfo impact = default)
        {
            IsCritical = isCritical;
            Amount = amount;
            Source = source;
            AttackerTransform = attackerTransform;
            Impact = impact;
        }

        public DamageInfo WithImpact(DamageImpactInfo impact)
        {
            return new DamageInfo(IsCritical, Amount, Source, AttackerTransform, impact);
        }
    }
}
