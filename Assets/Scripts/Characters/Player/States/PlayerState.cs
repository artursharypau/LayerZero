using Characters.Common;
using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Player.States
{
    public abstract class PlayerState : CharacterState<PlayerController>
    {
        protected PlayerState(
            StateMachine fsm,
            PlayerController controller,
            int hash,
            AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(fsm, controller, hash, type)
        {
        }

        public override bool TryTransition()
        {
            if (base.TryTransition())
            {
                return true;
            }

            if (Controller.CanDash())
            {
                FSM.ChangeState(Controller.DashState);

                return true;
            }

            return false;
        }
    }
}
