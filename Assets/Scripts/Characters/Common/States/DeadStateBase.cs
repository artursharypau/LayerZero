using LayerZero.Characters.Common.Animation;
using UnityEngine;

namespace LayerZero.Characters.Common.States
{
    public abstract class DeadStateBase<TCharacter> : CharacterState<TCharacter>
        where TCharacter : Character
    {
        protected DeadStateBase(TCharacter owner, AnimatorParameter parameter)
            : base(owner, parameter)
        {
        }

        protected DeadStateBase(TCharacter owner)
            : base(owner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Movement.Stop();

            Movement.SetGravityScale(0f);

            foreach (Collider2D collider in Owner.GetComponentsInChildren<Collider2D>())
            {
                collider.enabled = false;
            }

            OnDeath();
        }

        protected virtual void OnDeath()
        {
        }
    }
}
