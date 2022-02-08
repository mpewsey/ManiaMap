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

        public override string ToString()
        {
            return $"EdgeIndexes(FromIndex = {FromIndex}, ToIndex = {ToIndex})";
        }

        public override bool Equals(object obj)
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
            int hashCode = -1140728013;
            hashCode = hashCode * -1521134295 + FromIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + ToIndex.GetHashCode();
            return hashCode;
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
