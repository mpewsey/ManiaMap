using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing two Room ID's.
    /// </summary>
    [DataContract]
    public struct RoomPair : IEquatable<RoomPair>, IComparable<RoomPair>
    {
        /// <summary>
        /// The from room ID.
        /// </summary>
        [DataMember(Order = 1)]
        public Uid FromRoom { get; private set; }

        /// <summary>
        /// The to room ID.
        /// </summary>
        [DataMember(Order = 2)]
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

        public override string ToString()
        {
            return $"RoomPair(FromRoom = {FromRoom}, ToRoom = {ToRoom})";
        }

        public override bool Equals(object obj)
        {
            return obj is RoomPair pair && Equals(pair);
        }

        public bool Equals(RoomPair other)
        {
            return FromRoom.Equals(other.FromRoom) &&
                   ToRoom.Equals(other.ToRoom);
        }

        public override int GetHashCode()
        {
            int hashCode = -1486066407;
            hashCode = hashCode * -1521134295 + FromRoom.GetHashCode();
            hashCode = hashCode * -1521134295 + ToRoom.GetHashCode();
            return hashCode;
        }

        public int CompareTo(RoomPair other)
        {
            var comparison = FromRoom.CompareTo(other.FromRoom);

            if (comparison != 0)
                return comparison;

            return ToRoom.CompareTo(other.ToRoom);
        }

        public static bool operator ==(RoomPair left, RoomPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomPair left, RoomPair right)
        {
            return !(left == right);
        }
    }
}
