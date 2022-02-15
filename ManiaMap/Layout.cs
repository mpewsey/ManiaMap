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

        public bool Intersects(RoomTemplate template, int x, int y)
        {
            foreach (var room in Rooms.Values)
            {
                if (template.Intersects(room.Template, room.X - x, room.Y - y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
