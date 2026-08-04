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
        protected CharacterAnimator Animation => Owner.Animator;
        protected CharacterMovement2D Movement => Owner.Movement;

        public override void Enter()
        {
            Animation.Begin(_parameter);
        }

        public override void Exit()
        {
            Animation.End(_parameter);
        }

        protected void ChangeTo(int id, StateTransitionMode mode = StateTransitionMode.Deferred)
        {
            Owner.StateMachine.ChangeState(id, mode);
        }

        protected void ChangeTo<TPayload>(int id, TPayload payload, StateTransitionMode mode = StateTransitionMode.Deferred)
        {
            Owner.StateMachine.ChangeState(id, payload, mode);
        }
    }
}
