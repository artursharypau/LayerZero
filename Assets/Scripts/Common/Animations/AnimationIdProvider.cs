using UnityEngine;

namespace Common.Animations
{
    public static class AnimationIdProvider
    {
        public static readonly int Idle = Animator.StringToHash("idle");
        public static readonly int Move = Animator.StringToHash("move");
        public static readonly int Attack = Animator.StringToHash("attack");

        public static readonly int VelocityX = Animator.StringToHash("velocityX");
        public static readonly int VelocityY = Animator.StringToHash("velocityY");
    }
}
