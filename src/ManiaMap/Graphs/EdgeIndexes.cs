using System;

namespace MPewsey.ManiaMap.Graphs
{
    /// <summary>
    /// A pair of edge node indexes.
    /// </summary>
    public struct EdgeIndexes : IEquatable<EdgeIndexes>, IComparable<EdgeIndexes>
    {
        /// <summary>
        /// The from node index.
        /// </summary>
        public int FromIndex { get; set; }

        /// <summary>
        /// The two node index.
        /// </summary>
        public int ToIndex { get; set; }

        /// <summary>
        /// Initializes from two node index values.
        /// </summary>
        /// <param name="from">The from node index.</param>
        /// <param name="to">The to node index.</param>
        public EdgeIndexes(int from, int to)
        {
            FromIndex = from;
            ToIndex = to;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"EdgeIndexes(FromIndex = {FromIndex}, ToIndex = {ToIndex})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is EdgeIndexes indexes && Equals(indexes);
        }

        /// <inheritdoc/>
        public bool Equals(EdgeIndexes other)
        {
            return FromIndex == other.FromIndex &&
                   ToIndex == other.ToIndex;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1140728013;
            hashCode = hashCode * -1521134295 + FromIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + ToIndex.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public int CompareTo(EdgeIndexes other)
        {
            var comparison = FromIndex.CompareTo(other.FromIndex);

            if (comparison != 0)
                return comparison;

            return ToIndex.CompareTo(other.ToIndex);
        }

        /// <inheritdoc/>
        public static bool operator ==(EdgeIndexes left, EdgeIndexes right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(EdgeIndexes left, EdgeIndexes right)
        {
            return !(left == right);
        }
    }
}
