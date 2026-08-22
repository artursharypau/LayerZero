using System;

namespace LayerZero.Gameplay.Combat.Attack
{
    public interface IAttackParryWindowEvents
    {
        event Action AttackParryWindowOpened;
        event Action AttackParryWindowClosed;
    }
}
