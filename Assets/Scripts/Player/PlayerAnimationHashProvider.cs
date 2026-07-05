using UnityEngine;

namespace Player
{
    public static class PlayerAnimationHashProvider
    {
        public static readonly int JumpFall = Animator.StringToHash("jumpFall");
        public static readonly int WallSlide = Animator.StringToHash("wallSlide");
        public static readonly int Dash = Animator.StringToHash("dash");

        public static readonly int AttackIndex = Animator.StringToHash("attackIndex");
        public static readonly int JumpAttack = Animator.StringToHash("jumpAttack");
        public static readonly int JumpAttackTrigger = Animator.StringToHash("jumpAttackTrigger");
    }
}
