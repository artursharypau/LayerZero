using System;

namespace Characters.Common
{
    public interface IAttackFeedback
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
