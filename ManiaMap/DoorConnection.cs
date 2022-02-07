using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class DoorConnection
    {
        public Room FromRoom { get; }
        public Room ToRoom { get; }
        public Door FromDoor { get; }
        public Door ToDoor { get; }

        public DoorConnection(Room fromRoom, Room toRoom, Door fromDoor, Door toDoor)
        {
            FromRoom = fromRoom;
            ToRoom = toRoom;
            FromDoor = fromDoor;
            ToDoor = toDoor;
        }
    }
}
