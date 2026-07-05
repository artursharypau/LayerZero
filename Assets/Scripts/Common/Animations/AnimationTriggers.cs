using System;
using UnityEngine;

namespace Common.Animations
{
    public class AnimationTriggers : MonoBehaviour
    {
        public event Action AttackFinished = delegate { };

        private void AttackOver()
        {
            AttackFinished.Invoke();
        }
    }
}
