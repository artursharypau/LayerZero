using System;
using UnityEngine;

namespace LayerZero.Characters.Common.Animation
{
    public sealed class AnimatorEvents : MonoBehaviour, IAnimatorEvents
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
