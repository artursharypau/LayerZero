using LayerZero.Gameplay.Characters.Common.Animation;

namespace LayerZero.Gameplay.Characters.Player.Animation
{
    internal static class PlayerAnimatorParameters
    {
        public static readonly AnimatorParameter AttackIndex = new("attackIndex", AnimatorParameterKind.Int);
        public static readonly AnimatorParameter JumpAttackTrigger = new("jumpAttackTrigger", AnimatorParameterKind.Trigger);
        public static readonly AnimatorParameter CounterattackTrigger = new("counterattackTrigger", AnimatorParameterKind.Trigger);
    }
}
