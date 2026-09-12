namespace LayerZero.Gameplay.Combat.Elements
{
    internal sealed class ElementalAffinity : IElementalAffinity
    {
        public ElementKind Kind { get; private set; }

        public void Select(ElementKind kind)
        {
            Kind = kind;
        }
    }
}
