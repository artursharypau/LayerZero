using UnityEngine;

namespace Common.Animations
{
    public class AnimationContext
    {
        public int ParameterHash;
        public AnimatorParameterType ParameterType;
        public Animator Anim;

        public AnimationContext(int parameterHash, AnimatorParameterType parameterType, Animator anim)
        {
            ParameterHash = parameterHash;
            ParameterType = parameterType;
            Anim = anim;
        }
    }
}
