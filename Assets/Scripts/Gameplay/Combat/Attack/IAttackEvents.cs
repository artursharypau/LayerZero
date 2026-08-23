using System;

namespace LayerZero.Gameplay.Combat.Attack
{
    internal interface IAttackEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
