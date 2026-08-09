using LayerZero.Characters.Common.Animation;
using LayerZero.Characters.Common.Movement;
using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Common.States
{
    public abstract class CharacterState<TCharacter> : StateBase
        where TCharacter : Character2D
    {
        protected CharacterState(TCharacter owner)
        {
            Owner = owner;
        }

        protected TCharacter Owner { get; }
        protected CharacterAnimator Animator => Owner.Animator;
        protected IMovement2D Movement => Owner.Movement;
    }
}
