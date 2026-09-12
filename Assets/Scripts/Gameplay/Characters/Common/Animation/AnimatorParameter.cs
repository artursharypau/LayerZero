using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Animation
{
    internal readonly struct AnimatorParameter
    {
        public readonly int Hash;
        public readonly AnimatorParameterKind Kind;

        public AnimatorParameter(string name, AnimatorParameterKind kind)
        {
            Hash = Animator.StringToHash(name);
            Kind = kind;
        }
    }
}
