using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public class LayoutGenerator
    {
        public int Seed { get; set; }
        public int MaxRebases { get; set; }
        public int MaxBranchLength { get; set; }
        public LayoutGraph Graph { get; set; }
        public TemplateGroups TemplateGroups { get; set; }
        private Dictionary<TemplatePair, ConfigurationSpace> ConfigurationSpaces { get; set; }
        private List<List<LayoutEdge>> Chains { get; set; }
        private Random Random { get; set; }

        public LayoutGenerator(int seed, LayoutGraph graph, TemplateGroups templateGroups,
            int maxRebases = 100, int maxBranchLength = -1)
        {
            Seed = seed;
            MaxRebases = maxRebases;
            MaxBranchLength = maxBranchLength;
            Graph = graph;
            TemplateGroups = templateGroups;
        }

        public override string ToString()
        {
            return $"LayoutGenerator(Seed = {Seed})";
        }

        /// <summary>
        /// Generates a new layout.
        /// </summary>
        public Layout GenerateLayout()
        {
            int chain = 0;
            Random = new Random(Seed);
            Chains = Graph.FindChains(MaxBranchLength);
            ConfigurationSpaces = TemplateGroups.GetConfigurationSpaces();
            var layouts = new Stack<Layout>();
            layouts.Push(new Layout(Graph.Name, Seed));

            while (layouts.Count > 0)
            {
                var baseLayout = layouts.Peek();

                if (chain >= Chains.Count)
                {
                    return baseLayout;
                }

                // If layout has been used as base more than the maximum allowable,
                // remove the layout and backtrack.
                if (baseLayout.Rebases >= MaxRebases)
                {
                    layouts.Pop();
                    chain--;
                    continue;
                }

                // Create a new layout and try to add the next chain.
                var layout = new Layout(baseLayout);

                if (AddChain(layout, Chains[chain]))
                {
                    layouts.Push(layout);
                    chain++;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to add the specified chain index to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddChain(Layout layout, List<LayoutEdge> chain)
        {
            for (int i = 0; i < chain.Count; i++)
            {
                var backEdge = chain[i];
                var aheadEdge = i < chain.Count - 1 ? chain[i + 1] : null;

                if (CanInsertRoom(layout, backEdge, aheadEdge))
                {
                    var backRoom = layout.Rooms[backEdge.FromNode];
                    var aheadRoom = layout.Rooms[aheadEdge.ToNode];
                    var backDirection = backEdge.Direction;
                    var aheadDirection = aheadEdge.Direction;
                    var node = Graph.GetNode(backEdge.ToNode);
                    i++;

                    if (!InsertRoom(layout, node, backRoom, aheadRoom, backDirection, aheadDirection))
                    {
                        return false;
                    }
                }
                else if (!AddEdge(layout, backEdge))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if a room can be inserted between two rooms.
        /// </summary>
        private static bool CanInsertRoom(Layout layout, LayoutEdge backEdge, LayoutEdge aheadEdge)
        {
            return aheadEdge != null
                && backEdge.ToNode == aheadEdge.FromNode
                && layout.Rooms.ContainsKey(backEdge.FromNode)
                && layout.Rooms.ContainsKey(aheadEdge.ToNode)
                && !layout.Rooms.ContainsKey(backEdge.ToNode);
        }

        /// <summary>
        /// Attempts to add the specified edge to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddEdge(Layout layout, LayoutEdge edge)
        {
            var fromExists = layout.Rooms.TryGetValue(edge.FromNode, out var fromRoom);
            var toExists = layout.Rooms.TryGetValue(edge.ToNode, out var toRoom);

            if (!fromExists && !toExists)
            {
                var fromNode = Graph.GetNode(edge.FromNode);
                var toNode = Graph.GetNode(edge.ToNode);
                return AddFirstRoom(layout, fromNode)
                    && AddRoom(layout, toNode, layout.Rooms[edge.FromNode], edge.Direction);
            }

            if (!fromExists)
            {
                var node = Graph.GetNode(edge.FromNode);
                var direction = Door.ReverseEdgeDirection(edge.Direction);
                return AddRoom(layout, node, toRoom, direction);
            }

            if (!toExists)
            {
                var node = Graph.GetNode(edge.ToNode);
                return AddRoom(layout, node, fromRoom, edge.Direction);
            }

            return AddRoomConnection(layout, fromRoom, toRoom, edge.Direction);
        }

        /// <summary>
        /// Attempts to insert a new room to the layout between the specified rooms.
        /// Returns true if successful.
        /// </summary>
        private bool InsertRoom(Layout layout, LayoutNode node, Room backRoom, Room aheadRoom,
            EdgeDirection backDirection, EdgeDirection aheadDirection)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                var space = ConfigurationSpaces[new TemplatePair(backRoom.Template, template)];
                var configurations = new List<Configuration>(space.Configurations);
                Shuffle(configurations);

                foreach (var config in configurations)
                {
                    var x = config.X + backRoom.X;
                    var y = config.Y + backRoom.Y;
                    var z = node.Z - backRoom.Z;

                    if (!config.Matches(z, backDirection) || layout.Intersects(template, x, y, node.Z))
                        continue;

                    var newRoom = new Room(node, x, y, Random.Next(), template);

                    if (!AddRoomConnection(layout, newRoom, aheadRoom, aheadDirection))
                        continue;

                    if (!AddDoorConnection(layout, backRoom, newRoom, config.FromDoor, config.ToDoor))
                    {
                        // Remove the door connection added previously
                        layout.DoorConnections.RemoveAt(layout.DoorConnections.Count - 1);
                        continue;
                    }

                    layout.Rooms.Add(node.Id, newRoom);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new room to the layout attached to the specified room.
        /// Returns true if successful.
        /// </summary>
        private bool AddRoom(Layout layout, LayoutNode node, Room room, EdgeDirection direction)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                var space = ConfigurationSpaces[new TemplatePair(room.Template, template)];
                var configurations = new List<Configuration>(space.Configurations);
                Shuffle(configurations);

                foreach (var config in configurations)
                {
                    var x = config.X + room.X;
                    var y = config.Y + room.Y;
                    var z = node.Z - room.Z;

                    if (!config.Matches(z, direction) || layout.Intersects(template, x, y, node.Z))
                        continue;

                    var newRoom = new Room(node, x, y, Random.Next(), template);

                    if (AddDoorConnection(layout, room, newRoom, config.FromDoor, config.ToDoor))
                    {
                        layout.Rooms.Add(node.Id, newRoom);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to add the first room to the layout.
        /// </summary>
        private bool AddFirstRoom(Layout layout, LayoutNode node)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                if (layout.Intersects(template, 0, 0, node.Z))
                    return false;

                var newRoom = new Room(node, 0, 0, Random.Next(), template);
                layout.Rooms.Add(node.Id, newRoom);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to add the door connection to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddRoomConnection(Layout layout, Room from, Room to, EdgeDirection direction)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;
            var dz = to.Z - from.Z;
            var space = ConfigurationSpaces[new TemplatePair(from.Template, to.Template)];
            var configurations = new List<Configuration>(space.Configurations);
            Shuffle(configurations);

            foreach (var config in configurations)
            {
                if (!config.Matches(dx, dy, dz, direction))
                    continue;

                if (AddDoorConnection(layout, from, to, config.FromDoor, config.ToDoor))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Attemps to add a door connection to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddDoorConnection(Layout layout, Room from, Room to, Door fromDoor, Door toDoor)
        {
            if (Math.Abs(from.Z - to.Z) <= 1)
            {
                var connection = new DoorConnection(from.Id, to.Id, fromDoor, toDoor);
                layout.DoorConnections.Add(connection);
                return true;
            }

            var x = fromDoor.X + from.X;
            var y = fromDoor.Y + from.Y;
            var zMin = Math.Min(from.Z, to.Z) + 1;
            var zMax = Math.Max(from.Z, to.Z) - 1;

            if (layout.Intersects(x, x, y, y, zMin, zMax))
                return false;

            var shaft = new Box(x, x, y, y, zMin, zMax);
            var shaftConnection = new DoorConnection(from.Id, to.Id, fromDoor, toDoor, shaft);
            layout.DoorConnections.Add(shaftConnection);
            return true;
        }

        /// <summary>
        /// Shuffles the specified list in place.
        /// </summary>
        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var j = Random.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
