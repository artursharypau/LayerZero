using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Movement;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Common.States
{
    /// <summary>
    /// Base for every character state. Binds the state to its owner, exposes the handful of
    /// things states actually need, and turns the state's animator parameter on and off.
    /// </summary>
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

        /// <summary>Shorthand for the owner's state machine - states talk to each other by type.</summary>
        protected void ChangeTo<TState>(StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            Owner.States.ChangeState<TState>(mode);
        }

        protected void ChangeTo<TState, TPayload>(TPayload payload, StateTransitionMode mode = StateTransitionMode.Deferred)
            where TState : StateBase
        {
            Owner.States.ChangeState<TState, TPayload>(payload, mode);
        }
    }
}
