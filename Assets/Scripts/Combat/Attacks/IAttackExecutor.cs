using LayerZero.Combat.Damage;
using UnityEngine;

namespace LayerZero.Combat.Attacks
{
    public interface IAttackExecutor
    {
        AttackKind Kind { get; }

        void Initialize(Transform owner);
        bool IsInRange(Transform target);
        void Execute(DamageDefinition damage);
    }
}
