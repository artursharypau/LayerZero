using System;

namespace LayerZero.Combat.Attack
{
    public interface IAttackEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
