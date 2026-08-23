using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Animation
{
    internal sealed class CharacterAnimator
    {
        private readonly Animator _animator;

        public CharacterAnimator(Animator animator, IAnimatorEvents events)
        {
            _animator = animator;
            Events = events;
        }

        public IAnimatorEvents Events { get; }

        public void Trigger(in AnimatorParameter parameter)
        {
            if (_animator && parameter.Kind == AnimatorParameterKind.Trigger)
            {
                _animator.SetTrigger(parameter.Hash);
            }
        }

        public float GetFloat(in AnimatorParameter parameter)
        {
            return _animator ? _animator.GetFloat(parameter.Hash) : 0f;
        }

        public void SetFloat(in AnimatorParameter parameter, float value)
        {
            if (_animator)
            {
                _animator.SetFloat(parameter.Hash, value);
            }
        }

        public void SetInt(in AnimatorParameter parameter, int value)
        {
            if (_animator)
            {
                _animator.SetInteger(parameter.Hash, value);
            }
        }
    }
}
