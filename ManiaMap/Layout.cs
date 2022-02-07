using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Layout
    {
        public int Id { get; } = -1;
        public int Seed { get; }
        public Dictionary<int, Room> Rooms { get; } = new();
        public List<DoorConnection> DoorConnections { get; } = new();

        public Layout(int seed)
        {
            Seed = seed;
        }

        public Layout(Layout baseLayout)
        {
            Id = baseLayout.Id + 1;
            Seed = baseLayout.Seed;
            Rooms = new(baseLayout.Rooms);
            DoorConnections = new(baseLayout.DoorConnections);
        }

        public bool Intersects(RoomTemplate template, int dx, int dy)
        {
            foreach (var room in Rooms.Values)
            {
                if (template.Intersects(room.Template, room.X - dx, room.Y - dy))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
