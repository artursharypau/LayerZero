using LayerZero.Core.StateMachine;
using LayerZero.Gameplay.Characters.Common.Animation;
using LayerZero.Gameplay.Characters.Common.Movement;

namespace LayerZero.Gameplay.Characters.Common.States
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
