using System;

namespace LayerZero.Combat.Attacks
{
    /// <summary>
    /// Bridges animation events to gameplay: the animation decides *when* a hit lands
    /// and when the attack animation is over.
    /// </summary>
    public interface IAttackAnimatorEvents
    {
        event Action AttackHit;
        event Action AttackFinished;
    }
}
