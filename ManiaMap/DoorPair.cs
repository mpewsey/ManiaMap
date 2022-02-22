using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public struct DoorPair : IEquatable<DoorPair>
    {
        public DoorPosition FromDoor { get; }
        public DoorPosition ToDoor { get; }

        public DoorPair(DoorPosition from, DoorPosition to)
        {
            FromDoor = from;
            ToDoor = to;
        }

        public override string ToString()
        {
            return $"DoorPair(FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }

        public override bool Equals(object obj)
        {
            return obj is DoorPair pair && Equals(pair);
        }

        public bool Equals(DoorPair other)
        {
            return EqualityComparer<DoorPosition>.Default.Equals(FromDoor, other.FromDoor) &&
                   EqualityComparer<DoorPosition>.Default.Equals(ToDoor, other.ToDoor);
        }

        public override int GetHashCode()
        {
            int hashCode = -1667591795;
            hashCode = hashCode * -1521134295 + EqualityComparer<DoorPosition>.Default.GetHashCode(FromDoor);
            hashCode = hashCode * -1521134295 + EqualityComparer<DoorPosition>.Default.GetHashCode(ToDoor);
            return hashCode;
        }

        public static bool operator ==(DoorPair left, DoorPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DoorPair left, DoorPair right)
        {
            return !(left == right);
        }
    }
}
