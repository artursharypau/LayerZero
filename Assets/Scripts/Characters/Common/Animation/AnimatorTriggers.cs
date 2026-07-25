using System;
using Systems.Combat;
using UnityEngine;

namespace Characters.Common.Animation
{
    public class AnimatorTriggers : MonoBehaviour, IAttackAnimationEvents
    {
        public event Action AttackFinished;
        public event Action AttackHit;

        private void TriggerAttackFinished()
        {
            AttackFinished?.Invoke();
        }

        private void TriggerAttackHit()
        {
            AttackHit?.Invoke();
        }
    }
}
