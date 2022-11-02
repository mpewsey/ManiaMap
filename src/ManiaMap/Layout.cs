﻿using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a room layout consisting of Room and DoorConnection.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class Layout
    {
        /// <summary>
        /// The unique ID.
        /// </summary>
        [DataMember(Order = 0, IsRequired = true)]
        public int Id { get; private set; }

        /// <summary>
        /// The name of the layout.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The random seed used to generate the layout.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public RandomSeed Seed { get; private set; }

        /// <summary>
        /// A dictionary of rooms in the layout by ID.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public DataContractValueDictionary<Uid, Room> Rooms { get; private set; } = new DataContractValueDictionary<Uid, Room>();

        /// <summary>
        /// A dictionary of door connections by room ID pairs.
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        public DataContractValueDictionary<RoomPair, DoorConnection> DoorConnections { get; private set; } = new DataContractValueDictionary<RoomPair, DoorConnection>();

        /// <summary>
        /// A dictionary of room templates in the layout by ID.
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public DataContractValueDictionary<int, RoomTemplate> Templates { get; private set; } = new DataContractValueDictionary<int, RoomTemplate>();

        /// <summary>
        /// The current number of times the layout has been used as a base for another layout.
        /// </summary>
        public int Rebases { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            AssignRoomTemplates();
        }

        /// <summary>
        /// Initializes an empty layout.
        /// </summary>
        public Layout(int id, string name, RandomSeed seed)
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
            Rooms = new DataContractValueDictionary<Uid, Room>(baseLayout.Rooms);
            DoorConnections = new DataContractValueDictionary<RoomPair, DoorConnection>(baseLayout.DoorConnections);
            baseLayout.Rebases++;
            PopulateTemplates();
        }

        public override string ToString()
        {
            return $"Layout(Name = {Name}, Seed = {Seed})";
        }

        /// <summary>
        /// Populates the templates dictionary based on the current rooms.
        /// </summary>
        public void PopulateTemplates()
        {
            Templates.Clear();

            foreach (var room in Rooms.Values)
            {
                Templates[room.Template.Id] = room.Template;
            }
        }

        /// <summary>
        /// Sets the templates of the rooms based on their stored template ID's.
        /// </summary>
        private void AssignRoomTemplates()
        {
            foreach (var room in Rooms.Values)
            {
                room.SetTemplate(Templates.Dictionary);
            }
        }

        /// <summary>
        /// Returns the door connection between the rooms.
        /// </summary>
        /// <param name="room1">The first room ID.</param>
        /// <param name="room2">The second room ID.</param>
        public DoorConnection GetDoorConnection(Uid room1, Uid room2)
        {
            if (DoorConnections.TryGetValue(new RoomPair(room2, room1), out var connection))
                return connection;
            return DoorConnections[new RoomPair(room1, room2)];
        }

        /// <summary>
        /// Removes the door connection from the layout.
        /// </summary>
        /// <param name="room1">The first room ID.</param>
        /// <param name="room2">The second room ID.</param>
        public bool RemoveDoorConnection(Uid room1, Uid room2)
        {
            return DoorConnections.Remove(new RoomPair(room1, room2))
                || DoorConnections.Remove(new RoomPair(room2, room1));
        }

        /// <summary>
        /// Returns true if the range intersects the layout.
        /// </summary>
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        public bool Intersects(Vector3DInt min, Vector3DInt max)
        {
            return RoomsIntersect(min, max)
                || ShaftsIntersect(min, max);
        }

        /// <summary>
        /// Returns true if the range intersects the rooms of the layout.
        /// </summary>
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        private bool RoomsIntersect(Vector3DInt min, Vector3DInt max)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Intersects(min, max))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the range intersects the shafts of the layout.
        /// </summary>
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        private bool ShaftsIntersect(Vector3DInt min, Vector3DInt max)
        {
            foreach (var connection in DoorConnections.Values)
            {
                var shaft = connection.Shaft;

                if (shaft != null && shaft.Intersects(min, max))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the template intersects the layout.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="position">The position of the room.</param>
        /// <param name="z">The z position of the room.</param>
        public bool Intersects(RoomTemplate template, Vector2DInt position, int z)
        {
            return Intersects(template, new Vector3DInt(position.X, position.Y, z));
        }

        /// <summary>
        /// Returns true if the template intersects the layout.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="position">The position of the template.</param>
        public bool Intersects(RoomTemplate template, Vector3DInt position)
        {
            return RoomsIntersect(template, position)
                || ShaftsIntersect(template, position);
        }

        /// <summary>
        /// Returns true if the template intersects the rooms in the layout.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="position">The position of the template.</param>
        private bool RoomsIntersect(RoomTemplate template, Vector3DInt position)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Position.Z == position.Z
                    && template.Intersects(room.Template, room.Position - position))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the template intersects the shafts in the layout.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <param name="position">The position of the template.</param>
        private bool ShaftsIntersect(RoomTemplate template, Vector3DInt position)
        {
            foreach (var connection in DoorConnections.Values)
            {
                var shaft = connection.Shaft;

                if (shaft != null && shaft.Intersects(template, position))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a new dictionary of room doors by room ID.
        /// </summary>
        public Dictionary<Uid, List<DoorPosition>> GetRoomDoors()
        {
            var dict = new Dictionary<Uid, List<DoorPosition>>(Rooms.Count);

            foreach (var connection in DoorConnections.Values)
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

            foreach (var connection in DoorConnections.Values)
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
        public HashSet<Uid> FindCluster(Uid room, int maxDepth)
        {
            return new GraphClusterSearch<Uid>().FindCluster(RoomAdjacencies(), room, maxDepth);
        }

        /// <summary>
        /// Returns a dictionary of neighbors of all rooms up to the specified max depths.
        /// The rooms themselves are included in the results.
        /// </summary>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public Dictionary<Uid, HashSet<Uid>> FindClusters(int maxDepth)
        {
            return new GraphClusterSearch<Uid>().FindClusters(RoomAdjacencies(), maxDepth);
        }

        /// <summary>
        /// Returns the rectangular bounds of the layout.
        /// </summary>
        public RectangleInt GetBounds()
        {
            if (Rooms.Count == 0)
                return new RectangleInt();

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

            return new RectangleInt(minY, minX, maxY - minY, maxX - minX);
        }
    }
}
