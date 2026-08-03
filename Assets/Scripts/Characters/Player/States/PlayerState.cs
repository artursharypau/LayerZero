using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Abilities;
using LayerZero.Characters.Player.Config;
using LayerZero.Characters.Player.Input;

namespace LayerZero.Characters.Player.States
{
    /// <summary>
    /// Base for player states. Adds the shortcuts every player state needs and the one
    /// transition that is allowed from anywhere: the dash.
    /// </summary>
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
                ChangeTo<PlayerDashState>();
                return true;
            }

            return false;
        }
    }
}
