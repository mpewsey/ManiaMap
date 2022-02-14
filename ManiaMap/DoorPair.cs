using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public struct DoorPair : IEquatable<DoorPair>
    {
        public Door FromDoor { get; }
        public Door ToDoor { get; }

        public DoorPair(Door from, Door to)
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
            return EqualityComparer<Door>.Default.Equals(FromDoor, other.FromDoor) &&
                   EqualityComparer<Door>.Default.Equals(ToDoor, other.ToDoor);
        }

        public override int GetHashCode()
        {
            int hashCode = -1667591795;
            hashCode = hashCode * -1521134295 + EqualityComparer<Door>.Default.GetHashCode(FromDoor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Door>.Default.GetHashCode(ToDoor);
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
