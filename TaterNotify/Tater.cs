using System;

namespace TaterNotify
{
    class Tater : IEquatable<Tater>
    {
        public long PlayerId;
        public readonly string Batter;
        public readonly string Owner;
        public decimal Taters { get; set; }

        public Tater(long playerId, string batter, string owner, decimal taters)
        {
            PlayerId = playerId;
            Batter = batter;
            Owner = owner;
            Taters = taters;
        }

        public override string ToString()
        {
            return Taters + ": " + Batter + " [" + Owner + "]";
        }

        public bool Equals(Tater other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            return (other.PlayerId == PlayerId && other.Taters == Taters);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + PlayerId.GetHashCode();
                hash = hash * 23 + Taters.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tater);
        }

        public static bool operator ==(Tater left, Tater right)
        {
            if (Object.ReferenceEquals(left, null))
            {
                return Object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(Tater left, Tater right)
        {
            return !(left == right);
        }
    }
}
