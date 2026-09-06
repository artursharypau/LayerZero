using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal interface IDamageResolver
    {
        DamageInfo Resolve(DamageDefinition definition, Transform attackerTransform);
    }
}
