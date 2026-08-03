using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Movement;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Common.States
{
    public abstract class CharacterState<TCharacter> : StateBase
        where TCharacter : Character
    {
        private readonly AnimatorParameter _parameter;

        protected CharacterState(TCharacter owner, AnimatorParameter parameter)
        {
            Owner = owner;
            _parameter = parameter;
        }

        protected CharacterState(TCharacter owner)
            : this(owner, AnimatorParameter.None)
        {
        }

        protected TCharacter Owner { get; }
        protected CharacterAnimator Animation => Owner.Animation;
        protected CharacterMovement2D Movement => Owner.Movement;

        public override void Enter()
        {
            Animation.Begin(_parameter);
        }

        public override void Exit()
        {
            Animation.End(_parameter);
        }

        protected void ChangeTo<TState>(StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            Owner.StateMachine.ChangeState<TState>(mode);
        }

        protected void ChangeTo<TState, TPayload>(TPayload payload, StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            Owner.StateMachine.ChangeState<TState, TPayload>(payload, mode);
        }
    }
}
