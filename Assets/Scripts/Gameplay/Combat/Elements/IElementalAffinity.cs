namespace LayerZero.Gameplay.Combat.Elements
{
    internal interface IElementalAffinity
    {
        ElementKind Kind { get; }

        void Select(ElementKind kind);
    }
}
