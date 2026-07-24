using UnityEngine;

namespace Infrastructure.Animation
{
    public class AnimatorContext
    {
        public readonly int ParameterHash;
        public readonly AnimatorParameterType ParameterType;
        public readonly Animator Anim;

        public AnimatorContext(int parameterHash, AnimatorParameterType parameterType, Animator anim)
        {
            ParameterHash = parameterHash;
            ParameterType = parameterType;
            Anim = anim;
        }
    }
}
