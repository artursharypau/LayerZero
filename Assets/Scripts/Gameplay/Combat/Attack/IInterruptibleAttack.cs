using UnityEngine;

namespace LayerZero.Gameplay.Combat.Attack
{
    internal interface IInterruptibleAttack
    {
        bool TryInterrupt(Transform parrier);
    }
}
