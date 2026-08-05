namespace LayerZero.Combat.Damage.Resistance
{
    public readonly struct ResistanceHandle
    {
        public static readonly ResistanceHandle None = default;

        internal readonly int Id;

        internal ResistanceHandle(int id)
        {
            Id = id;
        }
    }
}
