using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Graphs;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a room layout consisting of Room and DoorConnection.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
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
        public DataContractValueDictionary<Uid, Room> Rooms { get; private set; } = new DataContractValueDictionary<Uid, Room>();

        /// <summary>
        /// A dictionary of door connections by room ID pairs.
        /// </summary>
        [DataMember(Order = 4)]
        public DataContractValueDictionary<RoomPair, DoorConnection> DoorConnections { get; private set; } = new DataContractValueDictionary<RoomPair, DoorConnection>();

        /// <summary>
        /// A dictionary of room templates in the layout by ID.
        /// </summary>
        [DataMember(Order = 5)]
        public DataContractValueDictionary<int, RoomTemplate> Templates { get; private set; } = new DataContractValueDictionary<int, RoomTemplate>();

        /// <summary>
        /// The current number of times the layout has been used as a base for another layout.
        /// </summary>
        public int Rebases { get; private set; }

        /// <summary>
        /// A dictionary of counts by template group entry.
        /// </summary>
        private Dictionary<TemplateGroupsEntry, int> TemplateCounts { get; set; } = new Dictionary<TemplateGroupsEntry, int>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Rooms = Rooms ?? new DataContractValueDictionary<Uid, Room>();
            DoorConnections = DoorConnections ?? new DataContractValueDictionary<RoomPair, DoorConnection>();
            Templates = Templates ?? new DataContractValueDictionary<int, RoomTemplate>();
            TemplateCounts = TemplateCounts ?? new Dictionary<TemplateGroupsEntry, int>();
            AssignRoomTemplates();
        }

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
            Rooms = new DataContractValueDictionary<Uid, Room>(baseLayout.Rooms);
            DoorConnections = new DataContractValueDictionary<RoomPair, DoorConnection>(baseLayout.DoorConnections);
            TemplateCounts = new Dictionary<TemplateGroupsEntry, int>(baseLayout.TemplateCounts);
            PopulateTemplates();
            baseLayout.Rebases++;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Layout(Id = {Id}, Name = {Name}, Seed = {Seed})";
        }

        /// <summary>
        /// Returns true if all template constraints are satisfied.
        /// </summary>
        /// <param name="groups">The template groups.</param>
        public bool IsComplete(TemplateGroups groups)
        {
            foreach (var entry in groups.GetAllEntries())
            {
                if (!entry.QuantitySatisfied(GetTemplateCount(entry)))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the count for the specified template group entry.
        /// </summary>
        /// <param name="entry">The template group entry.</param>
        public int GetTemplateCount(TemplateGroupsEntry entry)
        {
            TemplateCounts.TryGetValue(entry, out int count);
            return count;
        }

        /// <summary>
        /// Increases the count by 1 for the template group entry.
        /// </summary>
        /// <param name="entry">The template group entry.</param>
        public void IncreaseTemplateCount(TemplateGroupsEntry entry)
        {
            TemplateCounts[entry] = GetTemplateCount(entry) + 1;
        }

        /// <summary>
        /// Populates the templates dictionary based on the current rooms.
        /// </summary>
        public void PopulateTemplates()
        {
            Templates.Clear();

            foreach (var room in Rooms.Values)
            {
                if (!Templates.TryGetValue(room.Template.Id, out var template))
                    Templates.Add(room.Template.Id, room.Template);
                else if (!RoomTemplate.ValuesAreEqual(template, room.Template))
                    throw new DuplicateIdException($"Duplicate template ID: {room.Template.Id}.");
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
                var delta = room.Position - position;

                if (delta.Z == 0 && template.Intersects(room.Template, delta.To2D()))
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
        /// Returns a dictionary of door connections by room ID.
        /// </summary>
        public Dictionary<Uid, List<DoorConnection>> GetRoomConnections()
        {
            var dict = new Dictionary<Uid, List<DoorConnection>>(Rooms.Count);

            foreach (var connection in DoorConnections.Values)
            {
                if (!dict.TryGetValue(connection.FromRoom, out var fromConnections))
                {
                    fromConnections = new List<DoorConnection>();
                    dict.Add(connection.FromRoom, fromConnections);
                }

                if (!dict.TryGetValue(connection.ToRoom, out var toConnections))
                {
                    toConnections = new List<DoorConnection>();
                    dict.Add(connection.ToRoom, toConnections);
                }

                fromConnections.Add(connection);
                toConnections.Add(connection);
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

        /// <summary>
        /// Returns the ratio of visible cells over total cells.
        /// </summary>
        /// <param name="layoutState">The layout state.</param>
        public float VisibleCellProgress(LayoutState layoutState)
        {
            var counts = VisibleCellCount(layoutState);
            return counts.Y > 0 ? counts.X / (float)counts.Y : 1;
        }

        /// <summary>
        /// Returns a vector with of the visible cell count (X) and total cell count (Y).
        /// </summary>
        /// <param name="layoutState">The layout state.</param>
        public Vector2DInt VisibleCellCount(LayoutState layoutState)
        {
            var counts = Vector2DInt.Zero;

            foreach (var pair in Rooms)
            {
                var roomState = layoutState.RoomStates[pair.Key];
                counts += pair.Value.VisibleCellCount(roomState);
            }

            return counts;
        }

        /// <summary>
        /// Returns the first room with the specified tag. Returns null if the tag doesn't exist.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public Room FindRoomWithTag(string tag)
        {
            foreach (var room in Rooms.Values)
            {
                if (room.Tags.Contains(tag))
                    return room;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all rooms with the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public List<Room> FindRoomsWithTag(string tag)
        {
            var result = new List<Room>();
            FindRoomsWithTag(tag, ref result);
            return result;
        }

        /// <summary>
        /// Populates the referenced list with all rooms with the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="results">The referenced results list.</param>
        public void FindRoomsWithTag(string tag, ref List<Room> results)
        {
            results.Clear();

            foreach (var room in Rooms.Values)
            {
                if (room.Tags.Contains(tag))
                    results.Add(room);
            }
        }
    }
}
