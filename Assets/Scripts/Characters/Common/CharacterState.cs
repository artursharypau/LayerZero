using Infrastructure.Animation;
using Infrastructure.StateMachine;

namespace Characters.Common
{
    public abstract class CharacterState<TController> : State
        where TController : CharacterControllerBase
    {
        protected TController Controller { get; }

        protected CharacterState(
            StateMachine fsm,
            TController controller,
            int hash,
            AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(fsm, new AnimatorContext(hash, type, controller.Anim))
        {
            Controller = controller;
        }
    }
}
