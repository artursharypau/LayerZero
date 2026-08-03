using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.Animation
{
    public static class PlayerAnimatorParameters
    {
        public static readonly AnimatorParameter JumpFall = new("jumpFall", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter WallSlide = new("wallSlide", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter Dash = new("dash", AnimatorParameterKind.Bool);

        public static readonly AnimatorParameter AttackIndex = new("attackIndex", AnimatorParameterKind.Int);
        public static readonly AnimatorParameter JumpAttack = new("jumpAttack", AnimatorParameterKind.Bool);
        public static readonly AnimatorParameter JumpAttackLanding = new("jumpAttackTrigger", AnimatorParameterKind.Trigger);
    }
}
