using System.Collections.Generic;
using LayerZero.Combat.Damage;
using LayerZero.Core.Events;
using UnityEngine;

namespace LayerZero.Combat.Attack.Executors
{
    public interface IAttackExecutor
    {
        AttackKind Kind { get; }

        void Initialize(Transform owner, IEventBus eventBus);
        int FindTargets(List<Collider2D> results);
        bool IsInRange(Transform target);
        void Execute(DamageDefinition damage);
    }
}
