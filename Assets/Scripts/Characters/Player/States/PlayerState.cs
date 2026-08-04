using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    public abstract class PlayerState : CharacterState<PlayerController>
    {
        protected PlayerState(PlayerController owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        protected IPlayerInput Input => Owner.Input;
        protected PlayerConfig Config => Owner.Config;

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Owner.Abilities.CanUse(PlayerAbilityId.Dash))
            {
                Owner.StateMachine.ChangeState(PlayerStateId.Dash);
                return true;
            }

            return false;
        }
    }
}
