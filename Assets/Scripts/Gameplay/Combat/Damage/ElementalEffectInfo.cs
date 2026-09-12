using LayerZero.Gameplay.Combat.Elements;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal readonly struct ElementalEffectInfo
    {
        public readonly ElementKind Kind;
        public readonly float Value;
        public readonly float Duration;

        public ElementalEffectInfo(ElementKind kind, float value, float duration)
        {
            Kind = kind;
            Value = value;
            Duration = duration;
        }
    }
}
