using System;

namespace Systems.Combat
{
    public interface IAttackAnimatorEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
