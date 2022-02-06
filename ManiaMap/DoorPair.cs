using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public struct DoorPair
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
    }
}
