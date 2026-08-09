namespace LayerZero.Characters.Common.Animation
{
    public static class CommonAnimatorParameters
    {
        public static readonly AnimatorParameter Idle = new("idle", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter Move = new("move", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter Attack = new("attack", AnimatorParameterKind.Trigger);

        public static readonly AnimatorParameter Hurt = new("hurt", AnimatorParameterKind.Trigger);
        public static readonly AnimatorParameter Dead = new("dead", AnimatorParameterKind.Trigger);

        public static readonly AnimatorParameter VelocityX = new("velocityX", AnimatorParameterKind.Float);
        public static readonly AnimatorParameter VelocityY = new("velocityY", AnimatorParameterKind.Float);
    }
}
