using System;
using UnityEngine;

namespace Core.Animation
{
    public class AnimatorTriggers : MonoBehaviour
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
