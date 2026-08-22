using System;

namespace LayerZero.Gameplay.Combat.Attack
{
    public interface IAttackEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
