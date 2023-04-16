using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing two Room ID's.
    /// </summary>
    public struct RoomPair : IEquatable<RoomPair>, IComparable<RoomPair>
    {
        /// <summary>
        /// The from room ID.
        /// </summary>
        public Uid FromRoom { get; private set; }

        /// <summary>
        /// The to room ID.
        /// </summary>
        public Uid ToRoom { get; private set; }

        /// <summary>
        /// Initializes a new room pair.
        /// </summary>
        /// <param name="from">The from room ID.</param>
        /// <param name="to">The to room ID.</param>
        public RoomPair(Uid from, Uid to)
        {
            FromRoom = from;
            ToRoom = to;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"RoomPair(FromRoom = {FromRoom}, ToRoom = {ToRoom})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is RoomPair pair && Equals(pair);
        }

        /// <inheritdoc/>
        public bool Equals(RoomPair other)
        {
            return FromRoom.Equals(other.FromRoom) &&
                   ToRoom.Equals(other.ToRoom);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1486066407;
            hashCode = hashCode * -1521134295 + FromRoom.GetHashCode();
            hashCode = hashCode * -1521134295 + ToRoom.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public int CompareTo(RoomPair other)
        {
            var comparison = FromRoom.CompareTo(other.FromRoom);

            if (comparison != 0)
                return comparison;

            return ToRoom.CompareTo(other.ToRoom);
        }

        /// <inheritdoc/>
        public static bool operator ==(RoomPair left, RoomPair right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(RoomPair left, RoomPair right)
        {
            return !(left == right);
        }
    }
}
