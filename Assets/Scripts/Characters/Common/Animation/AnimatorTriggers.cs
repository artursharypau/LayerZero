using System;
using Systems.Combat;
using UnityEngine;

namespace Characters.Common.Animation
{
    public class AnimatorTriggers : MonoBehaviour, IAttackAnimatorEvents
    {
        public event Action AttackFinished;
        public event Action AttackHit;

        public void TriggerAttackFinished()
        {
            AttackFinished?.Invoke();
        }

        public void TriggerAttackHit()
        {
            AttackHit?.Invoke();
        }
    }
}
