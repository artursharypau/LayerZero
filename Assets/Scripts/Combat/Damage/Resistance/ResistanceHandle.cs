using System;

namespace LayerZero.Combat.Damage.Resistance
{
    public readonly struct ResistanceHandle : IEquatable<ResistanceHandle>
    {
        public static readonly ResistanceHandle None = default;

        internal readonly int Id;

        internal ResistanceHandle(int id)
        {
            Id = id;
        }

        public bool IsValid => Id != 0;

        public bool Equals(ResistanceHandle other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is ResistanceHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
