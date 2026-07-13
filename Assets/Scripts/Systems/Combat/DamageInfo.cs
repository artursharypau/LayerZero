using UnityEngine;

namespace Systems.Combat
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly DamageSource Source;
        public readonly Transform AttackerTransform;

        public DamageInfo(int amount, DamageSource source, Transform attackerTransform)
        {
            Amount = amount;
            Source = source;
            AttackerTransform = attackerTransform;
        }
    }
}
