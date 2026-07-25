using System;
using Characters.Common.Animation;
using UnityEngine;

namespace Core.StateMachine
{
    public abstract class State
    {
        private readonly int _parameterHash;
        private readonly AnimatorParameterType _parameterType;

        protected Animator Anim { get; }

        protected State(AnimatorContext animContext)
        {
            _parameterHash = animContext.ParameterHash;
            _parameterType = animContext.ParameterType;

            Anim = animContext.Anim;
        }

        public virtual void Enter()
        {
            switch (_parameterType)
            {
                case AnimatorParameterType.None:
                    break;
                case AnimatorParameterType.Bool:
                    Anim.SetBool(_parameterHash, true);
                    break;
                case AnimatorParameterType.Trigger:
                    Anim.SetTrigger(_parameterHash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual bool TryTransition()
        {
            return false;
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            if (_parameterType == AnimatorParameterType.Bool)
            {
                Anim.SetBool(_parameterHash, false);
            }
        }
    }
}
