namespace LayerZero.Gameplay.Combat.Damage.Protections
{
    internal readonly struct ProtectionHandle
    {
        public static readonly ProtectionHandle None = default;

        internal readonly int Id;

        internal ProtectionHandle(int id)
        {
            Id = id;
        }
    }
}
