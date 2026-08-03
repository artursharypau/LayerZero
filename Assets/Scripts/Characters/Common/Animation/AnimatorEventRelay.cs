using System;
using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Common.Animation
{
    public sealed class AnimatorEventRelay : MonoBehaviour, IAttackAnimatorEvents
    {
        public event Action AttackHit;
        public event Action AttackFinished;

        public void TriggerAttackHit()
        {
            AttackHit?.Invoke();
        }

        public void TriggerAttackFinished()
        {
            AttackFinished?.Invoke();
        }
    }
}
