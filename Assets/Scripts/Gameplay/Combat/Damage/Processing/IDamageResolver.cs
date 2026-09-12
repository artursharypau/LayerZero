using UnityEngine;

namespace LayerZero.Gameplay.Combat.Damage.Processing
{
    internal interface IDamageResolver
    {
        DamagePayload Resolve(DamageDefinition definition, Transform attackerTransform);
    }
}
