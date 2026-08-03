using LayerZero.Combat.Damage;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    /// <summary>
    /// Delivers damage for one <see cref="AttackKind" />. Implementations own their own
    /// geometry (hitbox, muzzle, range) - states and controllers never touch physics directly.
    /// </summary>
    public interface IAttackExecutor
    {
        AttackKind Kind { get; }

        void Initialize(Transform owner);

        /// <summary>Can this executor currently reach <paramref name="target" />?</summary>
        bool IsInRange(Transform target);

        void Execute(DamageDefinition damage);
    }
}
