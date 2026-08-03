using System;
using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Common.Animation
{
    /// <summary>
    /// Sits next to the <see cref="Animator" /> and turns animation events into C# events.
    /// <para>
    /// The <c>Trigger*</c> method names are referenced by the animation clips - do not rename
    /// them without updating the clips.
    /// </para>
    /// </summary>
    public sealed class AnimatorEventRelay : MonoBehaviour, IAttackAnimatorEvents
    {
        public event Action AttackHit;
        public event Action AttackFinished;

        /// <summary>Animation event: the exact frame the attack connects.</summary>
        public void TriggerAttackHit()
        {
            AttackHit?.Invoke();
        }

        /// <summary>Animation event: the attack animation is over, the state may leave.</summary>
        public void TriggerAttackFinished()
        {
            AttackFinished?.Invoke();
        }
    }
}
