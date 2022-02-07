using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Layout
    {
        public int Seed { get; }
        public int MaxRebases { get; }
        public int MaxBranchLength { get; }
        public int Rebases { get; private set; }
        public Dictionary<int, Room> Rooms { get; } = new();
        public List<DoorConnection> DoorConnections { get; } = new();

        public Layout(int seed, int maxRebases, int maxBranchLength)
        {
            Seed = seed;
            MaxRebases = maxRebases;
            MaxBranchLength = maxBranchLength;
        }

        public Layout(Layout baseLayout)
        {
            Seed = baseLayout.Seed;
            MaxRebases = baseLayout.Seed;
            MaxBranchLength = baseLayout.MaxBranchLength;
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
