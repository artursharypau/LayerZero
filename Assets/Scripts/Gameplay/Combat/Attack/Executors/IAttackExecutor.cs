using System.Collections.Generic;
using UnityEngine;

namespace LayerZero.Gameplay.Combat.Attack.Executors
{
    internal interface IAttackExecutor
    {
        AttackKind Kind { get; }

        int FindTargets(List<Collider2D> results);
        bool IsInRange(Transform target);
        void Execute(AttackDefinition attackDefinition);
    }
}
