namespace LayerZero.Gameplay.Combat.Damage.Protections
{
    internal readonly struct ProtectionHandle
    {
        public static readonly ProtectionHandle None = default;

        internal readonly int Id;
        internal readonly ProtectionKind Kind;

        internal ProtectionHandle(int id, ProtectionKind kind)
        {
            Id = id;
            Kind = kind;
        }
    }
}
