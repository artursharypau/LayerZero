using LayerZero.Gameplay.Combat.Attack;
using UnityEngine;

namespace LayerZero.Gameplay.Combat
{
    internal interface ICombatSystem
    {
        void Arm(AttackDefinition attack);
        void Disarm();
        bool IsInRange(AttackKind kind, Transform target);
        bool TryParry();
    }
}
