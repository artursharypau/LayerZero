using System;

namespace LayerZero.Combat.Attacks
{
    public interface IAttackEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
