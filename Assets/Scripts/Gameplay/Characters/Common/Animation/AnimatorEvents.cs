using System;
using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Animation
{
    public sealed class AnimatorEvents : MonoBehaviour, IAnimatorEvents
    {
        public event Action AttackHit;
        public event Action AttackFinished;
        public event Action AttackParryWindowOpened;
        public event Action AttackParryWindowClosed;

        public void TriggerAttackHit()
        {
            AttackHit?.Invoke();
        }

        public void TriggerAttackFinished()
        {
            AttackFinished?.Invoke();
        }

        public void TriggerAttackParryWindowOpened()
        {
            AttackParryWindowOpened?.Invoke();
        }

        public void TriggerAttackParryWindowClosed()
        {
            AttackParryWindowClosed?.Invoke();
        }
    }
}
