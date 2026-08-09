using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.Animation
{
    public static class PlayerAnimatorParameters
    {
        public static readonly AnimatorParameter AttackIndex = new("attackIndex", AnimatorParameterKind.Int);
        public static readonly AnimatorParameter JumpAttackTrigger = new("jumpAttackTrigger", AnimatorParameterKind.Trigger);
    }
}
