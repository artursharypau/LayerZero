using Common;
using UnityEngine;

namespace Player
{
    public static class PlayerAnimationIdProvider
    {
        public static readonly int Idle = AnimationIdProvider.Idle;
        public static readonly int Move = AnimationIdProvider.Move;
        public static readonly int JumpFall = Animator.StringToHash("jumpFall");
        public static readonly int WallSlide = Animator.StringToHash("wallSlide");
        public static readonly int Dash = Animator.StringToHash("dash");

        public static readonly int BasicAttack = Animator.StringToHash("basicAttack");
        public static readonly int[] BasicAttacks =
        {
            Animator.StringToHash("PlayerBasicAttack0"),
            Animator.StringToHash("PlayerBasicAttack1"),
            Animator.StringToHash("PlayerBasicAttack2")
        };
        public static readonly int JumpAttack = Animator.StringToHash("jumpAttack");
        public static readonly int JumpAttackTrigger = Animator.StringToHash("jumpAttackTrigger");
    }
}
