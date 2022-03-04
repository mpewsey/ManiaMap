using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public struct EdgeIndexes : IEquatable<EdgeIndexes>, IComparable<EdgeIndexes>
    {
        [DataMember(Order = 1)]
        public int FromIndex { get; set; }

        [DataMember(Order = 2)]
        public int ToIndex { get; set; }

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

        public int CompareTo(EdgeIndexes other)
        {
            var comparison = FromIndex.CompareTo(other.FromIndex);

            if (comparison != 0)
                return comparison;

            return ToIndex.CompareTo(other.ToIndex);
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
