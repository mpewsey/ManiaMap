using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    [DataContract]
    public class Layout
    {
        [DataMember(Order = 1)]
        public int Seed { get; private set; }

        [DataMember(Order = 2)]
        public Dictionary<int, Room> Rooms { get; private set; } = new();

        [DataMember(Order = 3)]
        public List<DoorConnection> DoorConnections { get; private set; } = new();

        public int Rebases { get; private set; }

        public Layout(int seed)
        {
            Seed = seed;
        }

        public Layout(Layout baseLayout)
        {
            Seed = baseLayout.Seed;
            Rooms = new(baseLayout.Rooms);
            DoorConnections = new(baseLayout.DoorConnections);
            baseLayout.Rebases++;
        }

        public override string ToString()
        {
            return $"Layout(Seed = {Seed})";
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
