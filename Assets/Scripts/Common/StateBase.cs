using System;
using Common.Animations;
using UnityEngine;

namespace Common
{
    public abstract class StateBase
    {
        private readonly int _parameterHash;
        private readonly AnimatorParameterType _parameterType;

        protected StateMachine FSM { get; }
        protected Animator Anim { get; }

        protected StateBase(StateMachine fsm, AnimationContext animContext)
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
                case AnimatorParameterType.Bool:
                    Anim.SetBool(_parameterHash, true);
                    break;
                case AnimatorParameterType.Trigger:
                    Anim.SetTrigger(_parameterHash);
                    break;
                case AnimatorParameterType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
