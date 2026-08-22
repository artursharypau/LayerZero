namespace LayerZero.Gameplay.Characters.Common.Animation
{
    public static class CommonAnimatorParameters
    {
        public static readonly AnimatorParameter State = new("state", AnimatorParameterKind.Int);

        public static readonly AnimatorParameter VelocityX = new("velocityX", AnimatorParameterKind.Float);
        public static readonly AnimatorParameter VelocityY = new("velocityY", AnimatorParameterKind.Float);

        public static readonly AnimatorParameter VelocityXAnimMultiplier = new("velocityXAnimMultiplier", AnimatorParameterKind.Float);
    }
}
