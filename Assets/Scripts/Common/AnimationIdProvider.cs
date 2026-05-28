using UnityEngine;

namespace Common
{
    public static class AnimationIdProvider
    {
        public static readonly int Idle = Animator.StringToHash("idle");
        public static readonly int Move = Animator.StringToHash("move");
    }
}
