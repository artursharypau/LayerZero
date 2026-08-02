using UnityEngine;

namespace Characters.Enemy.Animation
{
    public static class EnemyAnimatorHashProvider
    {
        public static readonly int Patrol = Animator.StringToHash("patrol");
        public static readonly int MoveAnimMultiplier = Animator.StringToHash("moveAnimMultiplier");
        public static readonly int Chase = Animator.StringToHash("chase");
        public static readonly int ChaseMoveAnimMultiplier = Animator.StringToHash("chaseMoveAnimMultiplier");
    }
}
