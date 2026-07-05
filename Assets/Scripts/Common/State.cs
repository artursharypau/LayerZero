using System;
using Common.Animations;
using UnityEngine;

namespace Common
{
    public abstract class State
    {
        private readonly int _parameterHash;
        private readonly AnimatorParameterType _parameterType;

        protected StateMachine FSM { get; }
        protected Animator Anim { get; }

        protected State(StateMachine fsm, AnimationContext animContext)
        {
            _parameterHash = animContext.ParameterHash;
            _parameterType = animContext.ParameterType;

            FSM = fsm;
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
