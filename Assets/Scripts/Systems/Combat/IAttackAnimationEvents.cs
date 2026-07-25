using System;

namespace Systems.Combat
{
    public interface IAttackAnimationEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
