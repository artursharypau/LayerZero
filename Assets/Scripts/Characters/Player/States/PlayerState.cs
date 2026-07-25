using Characters.Common;
using Characters.Player.Abilities;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public abstract class PlayerState : CharacterState<PlayerController>
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
