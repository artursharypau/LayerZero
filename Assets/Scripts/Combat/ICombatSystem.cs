using LayerZero.Combat.Attack;
using UnityEngine;

namespace LayerZero.Combat
{
    public interface ICombatSystem
    {
        void Arm(AttackDefinition attack);
        void Disarm();
        bool IsInRange(AttackKind kind, Transform target);
        bool TryParry();
    }
}
