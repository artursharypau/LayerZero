using UnityEngine;

namespace LayerZero.Combat.Attack
{
    public interface IInterruptibleAttack
    {
        bool TryInterrupt(Transform parrier);
    }
}
