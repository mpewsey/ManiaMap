using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Layout
    {
        [DataMember(Order = 0)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 1)]
        public int Seed { get; private set; }

        [DataMember(Order = 2)]
        public Dictionary<int, Room> Rooms { get; private set; } = new Dictionary<int, Room>();

        [DataMember(Order = 3)]
        public List<DoorConnection> DoorConnections { get; private set; } = new List<DoorConnection>();

        public int Rebases { get; private set; }

        public Layout(string name, int seed)
        {
            Name = name;
            Seed = seed;
        }

        public Layout(Layout baseLayout)
        {
            Name = baseLayout.Name;
            Seed = baseLayout.Seed;
            Rooms = new Dictionary<int, Room>(baseLayout.Rooms);
            DoorConnections = new List<DoorConnection>(baseLayout.DoorConnections);
            baseLayout.Rebases++;
        }

        public override string ToString()
        {
            return $"Layout(Name = {Name}, Seed = {Seed})";
        }

        /// <summary>
        /// Saves the layout to the file path.
        /// </summary>
        public void Save(string path)
        {
            var serializer = new DataContractSerializer(GetType());

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// Loads the layout from the file path.
        /// </summary>
        public static Layout Load(string path)
        {
            var serializer = new DataContractSerializer(typeof(Layout));

            using (var stream = File.OpenRead(path))
            {
                return (Layout)serializer.ReadObject(stream);
            }
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

        /// <summary>
        /// Returns a new dictionary of room doors by room ID.
        /// </summary>
        public Dictionary<int, List<DoorPosition>> RoomDoors()
        {
            var dict = new Dictionary<int, List<DoorPosition>>(Rooms.Count);

            foreach (var connection in DoorConnections)
            {
                if (!dict.TryGetValue(connection.FromRoom, out var fromDoors))
                {
                    fromDoors = new List<DoorPosition>();
                    dict.Add(connection.FromRoom, fromDoors);
                }

                if (!dict.TryGetValue(connection.ToRoom, out var toDoors))
                {
                    toDoors = new List<DoorPosition>();
                    dict.Add(connection.ToRoom, toDoors);
                }

                fromDoors.Add(connection.FromDoor);
                toDoors.Add(connection.ToDoor);
            }

            return dict;
        }

        /// <summary>
        /// Returns a new dictionary of neighbor rooms in the layout.
        /// </summary>
        public Dictionary<int, List<int>> RoomAdjacencies()
        {
            var dict = new Dictionary<int, List<int>>(Rooms.Count);

            foreach (var connection in DoorConnections)
            {
                if (!dict.TryGetValue(connection.FromRoom, out var fromNeighbors))
                {
                    fromNeighbors = new List<int>();
                    dict.Add(connection.FromRoom, fromNeighbors);
                }

                if (!dict.TryGetValue(connection.ToRoom, out var toNeighbors))
                {
                    toNeighbors = new List<int>();
                    dict.Add(connection.ToRoom, toNeighbors);
                }

                fromNeighbors.Add(connection.ToRoom);
                toNeighbors.Add(connection.FromRoom);
            }

            return dict;
        }

        /// <summary>
        /// Returns the neighbors of the room up to the specified max depth.
        /// </summary>
        public List<int> FindNeighbors(int room, int maxDepth)
        {
            return new LayoutNeighborSearch(this, maxDepth).FindNeighbors(room);
        }
    }
}
