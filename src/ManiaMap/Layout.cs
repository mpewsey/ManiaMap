using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a room layout consisting of `Room` and `DoorConnection`.
    /// </summary>
    [DataContract]
    public class Layout
    {
        /// <summary>
        /// The unique ID.
        /// </summary>
        [DataMember(Order = 0)]
        public int Id { get; private set; }

        /// <summary>
        /// The name of the layout.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The random seed used to generate the layout.
        /// </summary>
        [DataMember(Order = 2)]
        public int Seed { get; private set; }

        /// <summary>
        /// A dictionary of rooms in the layout by ID.
        /// </summary>
        [DataMember(Order = 3)]
        public Dictionary<Uid, Room> Rooms { get; private set; } = new Dictionary<Uid, Room>();

        /// <summary>
        /// A list of door connections in the layout.
        /// </summary>
        [DataMember(Order = 4)]
        public List<DoorConnection> DoorConnections { get; private set; } = new List<DoorConnection>();

        /// <summary>
        /// The current number of times the layout has been used as a base for another layout.
        /// </summary>
        public int Rebases { get; private set; }

        /// <summary>
        /// Initializes an empty layout.
        /// </summary>
        public Layout(int id, string name, int seed)
        {
            Id = id;
            Name = name;
            Seed = seed;
        }

        /// <summary>
        /// Initializes a new layout from a base.
        /// </summary>
        /// <param name="baseLayout">The base layout.</param>
        public Layout(Layout baseLayout)
        {
            Id = baseLayout.Id;
            Name = baseLayout.Name;
            Seed = baseLayout.Seed;
            Rooms = new Dictionary<Uid, Room>(baseLayout.Rooms);
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
        /// <param name="path">The save file path.</param>
        public void SaveXml(string path)
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
        /// <param name="path">The file path.</param>
        public static Layout LoadXml(string path)
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
        /// <param name="xMin">The minimum x value in the range.</param>
        /// <param name="xMax">The maximum x value in the range.</param>
        /// <param name="yMin">The minimum y value in the range.</param>
        /// <param name="yMax">The maximum y value in the range.</param>
        /// <param name="zMin">The minimum z value in the range.</param>
        /// <param name="zMax">The maximum z value in the range.</param>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Position.Z >= zMin && room.Position.Z <= zMax)
                {
                    var x1 = xMin - room.Position.X;
                    var x2 = xMax - room.Position.X;
                    var y1 = yMin - room.Position.Y;
                    var y2 = yMax - room.Position.Y;

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
        /// <param name="template">The room template.</param>
        /// <param name="x">The x position of the room template.</param>
        /// <param name="y">The y position of the room template.</param>
        /// <param name="z">The z position of the room template.</param>
        public bool Intersects(RoomTemplate template, int x, int y, int z)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Position.Z == z)
                {
                    var dx = room.Position.X - x;
                    var dy = room.Position.Y - y;

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
        public Dictionary<Uid, List<DoorPosition>> RoomDoors()
        {
            var dict = new Dictionary<Uid, List<DoorPosition>>(Rooms.Count);

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
        public Dictionary<Uid, List<Uid>> RoomAdjacencies()
        {
            var dict = new Dictionary<Uid, List<Uid>>(Rooms.Count);

            foreach (var connection in DoorConnections)
            {
                if (!dict.TryGetValue(connection.FromRoom, out var fromNeighbors))
                {
                    fromNeighbors = new List<Uid>();
                    dict.Add(connection.FromRoom, fromNeighbors);
                }

                if (!dict.TryGetValue(connection.ToRoom, out var toNeighbors))
                {
                    toNeighbors = new List<Uid>();
                    dict.Add(connection.ToRoom, toNeighbors);
                }

                fromNeighbors.Add(connection.ToRoom);
                toNeighbors.Add(connection.FromRoom);
            }

            return dict;
        }

        /// <summary>
        /// Returns the neighbors of the room up to the specified max depth.
        /// The room itself is included in the result.
        /// </summary>
        /// <param name="room">The room ID.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public List<Uid> FindNeighbors(Uid room, int maxDepth)
        {
            return new LayoutNeighborSearch(this, maxDepth).FindNeighbors(room);
        }

        /// <summary>
        /// Returns the rectangular bounds of the layout.
        /// </summary>
        public Rectangle Bounds()
        {
            if (Rooms.Count == 0)
                return new Rectangle();

            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var room in Rooms.Values)
            {
                minX = Math.Min(minX, room.Position.X);
                minY = Math.Min(minY, room.Position.Y);
                maxX = Math.Max(maxX, room.Position.X + room.Template.Cells.Rows);
                maxY = Math.Max(maxY, room.Position.Y + room.Template.Cells.Columns);
            }

            return new Rectangle(minY, minX, maxY - minY, maxX - minX);
        }
    }
}
