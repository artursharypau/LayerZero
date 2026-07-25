using Characters.Common.Animation;
using Characters.Common.States;
using Characters.Player.Abilities;

namespace Characters.Player.States
{
    public abstract class PlayerState : AnimatedState<PlayerController>
    {
        protected PlayerState(PlayerController controller, int hash, AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(controller, hash, type)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanUseAbility(PlayerAbilityId.Dash))
            {
                Controller.ChangeState(StateId.Dash);
                return true;
            }

            return false;
        }
    }
}
