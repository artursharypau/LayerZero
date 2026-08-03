using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Enemies.Animation
{
    public static class EnemyAnimatorParameters
    {
        public static readonly AnimatorParameter Patrol = new("patrol", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter Chase = new("chase", AnimatorParameterKind.Bool);

        public static readonly AnimatorParameter MoveAnimationMultiplier = new("moveAnimMultiplier", AnimatorParameterKind.Float);
        public static readonly AnimatorParameter ChaseAnimationMultiplier = new("chaseMoveAnimMultiplier", AnimatorParameterKind.Float);
    }
}
