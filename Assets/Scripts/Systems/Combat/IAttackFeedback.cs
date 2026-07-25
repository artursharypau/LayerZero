using System;

namespace Systems.Combat
{
    public interface IAttackFeedback
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
