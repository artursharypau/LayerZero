using System;

namespace LayerZero.Combat.Attack
{
    public interface IAttackParryWindowEvents
    {
        event Action AttackParryWindowOpened;
        event Action AttackParryWindowClosed;
    }
}
