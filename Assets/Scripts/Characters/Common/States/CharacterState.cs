using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Movement;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Common.States
{
    public abstract class CharacterState<TCharacter> : StateBase
        where TCharacter : Character2D
    {
        private readonly AnimatorParameter _parameter;

        protected CharacterState(TCharacter owner, AnimatorParameter parameter)
        {
            Owner = owner;

            _parameter = parameter;
        }

        protected TCharacter Owner { get; }
        protected CharacterAnimator Animator => Owner.Animator;
        protected IMovement2D Movement => Owner.Movement;

        public override void Enter()
        {
            Animator.Enter(_parameter);
        }

        public override void Exit()
        {
            Animator.Exit(_parameter);
        }
    }
}
