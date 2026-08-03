using System;
using Characters.Common.Animation;
using Core.StateMachine;
using UnityEngine;

namespace Characters.Common.States
{
    public abstract class AnimatedState<TController> : State
        where TController : CharacterController2D
    {
        private readonly int _parameterHash;
        private readonly AnimatorParameterType _parameterType;

        protected TController Controller { get; }
        protected Animator Anim { get; }

        protected AnimatedState(TController controller, int parameterHash, AnimatorParameterType parameterType)
        {
            Controller = controller;
            Anim = controller.Anim;

            _parameterHash = parameterHash;
            _parameterType = parameterType;
        }

        public override void Enter()
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

        public override void Exit()
        {
            switch (_parameterType)
            {
                case AnimatorParameterType.None:
                    break;
                case AnimatorParameterType.Bool:
                    Anim.SetBool(_parameterHash, false);
                    break;
                case AnimatorParameterType.Trigger:
                    Anim.ResetTrigger(_parameterHash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
