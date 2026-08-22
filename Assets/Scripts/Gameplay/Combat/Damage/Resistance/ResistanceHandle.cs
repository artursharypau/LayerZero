namespace LayerZero.Gameplay.Combat.Damage.Resistance
{
    public readonly struct ResistanceHandle
    {
        public static readonly ResistanceHandle None = default;

        internal readonly int Id;
        internal readonly ResistanceKind Kind;

        internal ResistanceHandle(int id, ResistanceKind kind)
        {
            Id = id;
            Kind = kind;
        }
    }
}
