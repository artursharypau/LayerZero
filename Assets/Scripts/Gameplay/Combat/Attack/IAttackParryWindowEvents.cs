using System;

namespace LayerZero.Gameplay.Combat.Attack
{
    internal interface IAttackParryWindowEvents
    {
        event Action AttackParryWindowOpened;
        event Action AttackParryWindowClosed;
    }
}
