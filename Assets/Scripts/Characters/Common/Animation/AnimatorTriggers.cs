using System;
using UnityEngine;

namespace Characters.Common.Animation
{
    public class AnimatorTriggers : MonoBehaviour, IAttackFeedback
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
