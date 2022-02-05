using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public struct EdgeIndexes : IEquatable<EdgeIndexes>
    {
        public int FromIndex { get; }
        public int ToIndex { get; }

        public EdgeIndexes(int from, int to)
        {
            FromIndex = from;
            ToIndex = to;
        }

        public override bool Equals(object? obj)
        {
            return obj is EdgeIndexes indexes && Equals(indexes);
        }

        public bool Equals(EdgeIndexes other)
        {
            return FromIndex == other.FromIndex &&
                   ToIndex == other.ToIndex;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FromIndex, ToIndex);
        }

        public static bool operator ==(EdgeIndexes left, EdgeIndexes right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EdgeIndexes left, EdgeIndexes right)
        {
            return !(left == right);
        }
    }
}
