using UnityEngine;

namespace LayerZero.Characters.Common.Animation
{
    /// <summary>
    /// A named animator parameter resolved to its hash once, together with the kind of
    /// parameter it is. Carrying the kind lets a state enter/exit generically instead of
    /// every state remembering whether its parameter is a bool or a trigger.
    /// </summary>
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

        public bool IsDefined => Kind != AnimatorParameterKind.None;
    }
}
