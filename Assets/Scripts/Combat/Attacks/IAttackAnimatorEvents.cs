using System;

namespace LayerZero.Combat.Attacks
{
    public interface IAttackAnimatorEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
