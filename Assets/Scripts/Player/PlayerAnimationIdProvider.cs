using UnityEngine;

namespace Player
{
    public static class PlayerAnimationIdProvider
    {
        public static readonly int JumpFall = Animator.StringToHash("jumpFall");
        public static readonly int WallSlide = Animator.StringToHash("wallSlide");
        public static readonly int Dash = Animator.StringToHash("dash");

        public static readonly int[] Attacks =
        {
            Animator.StringToHash("PlayerAttack0"),
            Animator.StringToHash("PlayerAttack1"),
            Animator.StringToHash("PlayerAttack2")
        };
        public static readonly int JumpAttack = Animator.StringToHash("jumpAttack");
        public static readonly int JumpAttackTrigger = Animator.StringToHash("jumpAttackTrigger");
    }
}
