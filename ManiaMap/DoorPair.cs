using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Returns the edge direction for the door pair.
        /// </summary>
        public EdgeDirection EdgeDirection()
        {
            return Door.GetEdgeDirection(FromDoor.Type, ToDoor.Type);
        }
    }
}
