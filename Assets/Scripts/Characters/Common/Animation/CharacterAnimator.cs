using System;
using LayerZero.Combat.Attacks;
using UnityEngine;

namespace LayerZero.Characters.Common.Animation
{
    public sealed class CharacterAnimator
    {
        private readonly Animator _animator;
        private readonly IAttackAnimatorEvents _events;

        public CharacterAnimator(Animator animator, IAttackAnimatorEvents events)
        {
            _animator = animator;
            _events = events;
        }

        public bool IsValid => _animator;

        public event Action AttackHit
        {
            add
            {
                if (_events != null)
                {
                    _events.AttackHit += value;
                }
            }
            remove
            {
                if (_events != null)
                {
                    _events.AttackHit -= value;
                }
            }
        }

        public event Action AttackFinished
        {
            add
            {
                if (_events != null)
                {
                    _events.AttackFinished += value;
                }
            }
            remove
            {
                if (_events != null)
                {
                    _events.AttackFinished -= value;
                }
            }
        }

        public void Begin(in AnimatorParameter parameter)
        {
            if (!_animator)
            {
                return;
            }

            switch (parameter.Kind)
            {
                case AnimatorParameterKind.Bool:
                    _animator.SetBool(parameter.Hash, true);
                    break;
                case AnimatorParameterKind.Trigger:
                    _animator.SetTrigger(parameter.Hash);
                    break;
            }
        }

        public void End(in AnimatorParameter parameter)
        {
            if (!_animator)
            {
                return;
            }

            switch (parameter.Kind)
            {
                case AnimatorParameterKind.Bool:
                    _animator.SetBool(parameter.Hash, false);
                    break;
                case AnimatorParameterKind.Trigger:
                    _animator.ResetTrigger(parameter.Hash);
                    break;
            }
        }

        public void Fire(in AnimatorParameter parameter)
        {
            if (_animator && parameter.Kind == AnimatorParameterKind.Trigger)
            {
                _animator.SetTrigger(parameter.Hash);
            }
        }

        public void SetFloat(in AnimatorParameter parameter, float value)
        {
            if (_animator)
            {
                _animator.SetFloat(parameter.Hash, value);
            }
        }

        public float GetFloat(in AnimatorParameter parameter)
        {
            return _animator ? _animator.GetFloat(parameter.Hash) : 0f;
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
