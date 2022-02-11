using System;
using System.Collections.Generic;
using System.Text;

namespace MPewsey.ManiaMap
{
    public struct RoomDoorPair : IEquatable<RoomDoorPair>
    {
        public Room Room { get; }
        public Door Door { get; }

        public RoomDoorPair(Room room, Door door)
        {
            Room = room;
            Door = door;
        }

        public override string ToString()
        {
            return $"RoomDoorPair(Room = {Room}, Door = {Door})";
        }

        public override bool Equals(object obj)
        {
            return obj is RoomDoorPair pair && Equals(pair);
        }

        public bool Equals(RoomDoorPair other)
        {
            return EqualityComparer<Room>.Default.Equals(Room, other.Room) &&
                   EqualityComparer<Door>.Default.Equals(Door, other.Door);
        }

        public override int GetHashCode()
        {
            int hashCode = 430290675;
            hashCode = hashCode * -1521134295 + EqualityComparer<Room>.Default.GetHashCode(Room);
            hashCode = hashCode * -1521134295 + EqualityComparer<Door>.Default.GetHashCode(Door);
            return hashCode;
        }

        public static bool operator ==(RoomDoorPair left, RoomDoorPair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomDoorPair left, RoomDoorPair right)
        {
            return !(left == right);
        }
    }
}
