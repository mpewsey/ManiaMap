using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Layout
    {
        [DataMember(Order = 1)]
        public int Seed { get; private set; }

        [DataMember(Order = 2)]
        public Dictionary<int, Room> Rooms { get; private set; } = new Dictionary<int, Room>();

        [DataMember(Order = 3)]
        public List<DoorConnection> DoorConnections { get; private set; } = new List<DoorConnection>();

        public int Rebases { get; private set; }

        public Layout(int seed)
        {
            Seed = seed;
        }

        public Layout(Layout baseLayout)
        {
            Seed = baseLayout.Seed;
            Rooms = new Dictionary<int, Room>(baseLayout.Rooms);
            DoorConnections = new List<DoorConnection>(baseLayout.DoorConnections);
            baseLayout.Rebases++;
        }

        public override string ToString()
        {
            return $"Layout(Seed = {Seed})";
        }

        /// <summary>
        /// Returns true if the range intersects the layout.
        /// </summary>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Z >= zMin && room.Z <= zMax)
                {
                    var x1 = xMin - room.X;
                    var x2 = xMax - room.X;
                    var y1 = yMin - room.Y;
                    var y2 = yMax - room.Y;

                    if (room.Template.Intersects(x1, x2, y1, y2))
                    {
                        return true;
                    }
                }
            }

            foreach (var connection in DoorConnections)
            {
                var shaft = connection.Shaft;

                if (shaft != null && shaft.Intersects(xMin, xMax, yMin, yMax, zMin, zMax))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the template intersects the layout.
        /// </summary>
        public bool Intersects(RoomTemplate template, int x, int y, int z)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Z == z)
                {
                    var dx = room.X - x;
                    var dy = room.Y - y;

                    if (template.Intersects(room.Template, dx, dy))
                    {
                        return true;
                    }
                }
            }

            foreach (var connection in DoorConnections)
            {
                var shaft = connection.Shaft;

                if (shaft != null && shaft.Intersects(template, x, y, z))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
