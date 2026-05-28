using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        public event Action AttackFinished = delegate { };

        private void AttackOver()
        {
            AttackFinished.Invoke();
        }
    }
}
