using UnityEngine;

namespace LayerZero.Gameplay.Combat.Attack
{
    public interface IInterruptibleAttack
    {
        bool TryInterrupt(Transform parrier);
    }
}
