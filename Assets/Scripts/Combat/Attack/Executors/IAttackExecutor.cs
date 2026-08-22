using System.Collections.Generic;
using LayerZero.Combat.Damage;
using UnityEngine;

namespace LayerZero.Combat.Attack.Executors
{
    public interface IAttackExecutor
    {
        AttackKind Kind { get; }

        int FindTargets(List<Collider2D> results);
        bool IsInRange(Transform target);
        void Execute(DamageDefinition damage);
    }
}
