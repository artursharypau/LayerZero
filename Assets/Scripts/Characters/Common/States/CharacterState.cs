using Characters.Common.Animation;
using Core.StateMachine;

namespace Characters.Common.States
{
    public abstract class CharacterState<TController> : State
        where TController : CharacterControllerBase
    {
        protected TController Controller { get; }

        protected CharacterState(TController controller, int hash, AnimatorParameterType type = AnimatorParameterType.Bool)
            : base(new AnimatorContext(hash, type, controller.Anim))
        {
            Controller = controller;
        }
    }
}
