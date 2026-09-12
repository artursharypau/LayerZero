using LayerZero.Gameplay.Combat.Elements;

namespace LayerZero.Gameplay.Combat.Damage
{
    internal readonly struct DamagePayload
    {
        public readonly DamageInfo Damage;
        public readonly ElementalEffectInfo ElementalEffect;

        public DamagePayload(DamageInfo damage, ElementalEffectInfo elementalEffect)
        {
            Damage = damage;
            ElementalEffect = elementalEffect;
        }

        public bool HasElementalEffect =>
            ElementalEffect.Kind != ElementKind.None && ElementalEffect.Value > 0f && ElementalEffect.Duration > 0f;
    }
}
