using UnityEngine;

namespace LayerZero.Gameplay.Characters.Common.Animation
{
    public readonly struct AnimatorParameter
    {
        public static readonly AnimatorParameter None = default;

        public readonly int Hash;
        public readonly AnimatorParameterKind Kind;

        public AnimatorParameter(string name, AnimatorParameterKind kind)
        {
            Hash = Animator.StringToHash(name);
            Kind = kind;
        }
    }
}
