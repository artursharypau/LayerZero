using UnityEngine;

namespace Enemy
{
    public static class EnemyAnimationIdProvider
    {
        public static readonly int MoveAnimMultiplier = Animator.StringToHash("moveAnimMultiplier");
        public static readonly int Battle = Animator.StringToHash("battle");
        public static readonly int BattleMoveAnimMultiplier = Animator.StringToHash("battleMoveAnimMultiplier");
    }
}
