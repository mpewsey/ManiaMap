using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ManiaMap
{
    [DataContract]
    public class DoorConnection
    {
        [DataMember(Order = 1)]
        public Room FromRoom { get; private set; }

        [DataMember(Order = 2)]
        public Room ToRoom { get; private set; }

        [DataMember(Order = 3)]
        public Door FromDoor { get; private set; }

        [DataMember(Order = 4)]
        public Door ToDoor { get; private set; }

        public DoorConnection(Room fromRoom, Room toRoom, Door fromDoor, Door toDoor)
        {
            FromRoom = fromRoom;
            ToRoom = toRoom;
            FromDoor = fromDoor;
            ToDoor = toDoor;
        }

        public override string ToString()
        {
            return $"DoorConnection(FromRoom = {FromRoom}, ToRoom = {ToRoom}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }
    }
}
