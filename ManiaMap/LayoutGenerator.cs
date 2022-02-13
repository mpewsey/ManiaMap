using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public class LayoutGenerator
    {
        private int Seed { get; }
        private int MaxRebases { get; }
        private int MaxBranchLength { get; }
        private LayoutGraph Graph { get; }
        private TemplateGroups TemplateGroups { get; }
        private Dictionary<TemplatePair, ConfigurationSpace> ConfigurationSpaces { get; set; }
        private List<List<LayoutEdge>> Chains { get; set; }
        private Random Random { get; set; }

        public LayoutGenerator(int seed, LayoutGraph graph, TemplateGroups templateGroups, int maxRebases = 1000, int maxBranchLength = 3)
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
            layouts.Push(new Layout(Seed));

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

                if (AddChain(layout, chain))
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
        private bool AddChain(Layout layout, int index)
        {
            var chain = Chains[index];

            for (int i = 0; i < chain.Count; i++)
            {
                if (CanInsertRoom(layout, chain, i))
                {
                    var backEdge = chain[i];
                    var aheadEdge = chain[i + 1];
                    var backRoom = layout.Rooms[backEdge.FromNode];
                    var aheadRoom = layout.Rooms[aheadEdge.ToNode];
                    var node = Graph.GetNode(backEdge.ToNode);
                    i++;

                    if (!InsertRoom(layout, backRoom, aheadRoom, node, backEdge.Direction, aheadEdge.Direction))
                    {
                        return false;
                    }
                }
                else if (!AddEdge(layout, chain[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if a room can be inserted between two rooms.
        /// </summary>
        private bool CanInsertRoom(Layout layout, List<LayoutEdge> chain, int index)
        {
            if (index >= chain.Count - 1)
                return false;

            var backEdge = chain[index];
            var aheadEdge = chain[index + 1];

            if (backEdge.ToNode != aheadEdge.FromNode)
                return false;
            if (!layout.Rooms.ContainsKey(backEdge.FromNode))
                return false;
            if (!layout.Rooms.ContainsKey(aheadEdge.ToNode))
                return false;
            if (layout.Rooms.ContainsKey(backEdge.ToNode))
                return false;

            return true;
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
                return AddFirstRoom(layout, fromNode) && AddRoom(layout, layout.Rooms[edge.FromNode], toNode, edge.Direction);
            }

            if (!fromExists)
            {
                var node = Graph.GetNode(edge.FromNode);
                var direction = Door.ReverseEdgeDirection(edge.Direction);
                return AddRoom(layout, toRoom, node, direction);
            }

            if (!toExists)
            {
                var node = Graph.GetNode(edge.ToNode);
                return AddRoom(layout, fromRoom, node, edge.Direction);
            }

            return AddDoorConnection(layout, fromRoom, toRoom, edge.Direction);
        }

        /// <summary>
        /// Attempts to insert a new room to the layout between the specified rooms.
        /// Returns true if successful.
        /// </summary>
        private bool InsertRoom(Layout layout, Room backRoom, Room aheadRoom, LayoutNode node, EdgeDirection backDirection, EdgeDirection aheadDirection)
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

                    if (config.EdgeDirection == backDirection && !layout.Intersects(template, x, y))
                    {
                        var newRoom = new Room(node.Id, x, y, template);

                        if (AddDoorConnection(layout, newRoom, aheadRoom, aheadDirection))
                        {
                            var connection = new DoorConnection(backRoom, newRoom, config.FromDoor, config.ToDoor);
                            layout.Rooms.Add(node.Id, newRoom);
                            layout.DoorConnections.Add(connection);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new room to the layout attached to the specified room.
        /// Returns true if successful.
        /// </summary>
        private bool AddRoom(Layout layout, Room room, LayoutNode node, EdgeDirection direction)
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

                    if (config.EdgeDirection == direction && !layout.Intersects(template, x, y))
                    {
                        var newRoom = new Room(node.Id, x, y, template);
                        var connection = new DoorConnection(room, newRoom, config.FromDoor, config.ToDoor);
                        layout.Rooms.Add(node.Id, newRoom);
                        layout.DoorConnections.Add(connection);
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
                if (!layout.Intersects(template, 0, 0))
                {
                    layout.Rooms.Add(node.Id, new Room(node.Id, 0, 0, template));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to add the door connection to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddDoorConnection(Layout layout, Room from, Room to, EdgeDirection direction)
        {
            var dx = to.X - from.X;
            var dy = to.Y - from.Y;
            var space = ConfigurationSpaces[new TemplatePair(from.Template, to.Template)];
            var configurations = new List<Configuration>(space.Configurations);
            Shuffle(configurations);

            foreach (var config in configurations)
            {
                if (config.X == dx && config.Y == dy && config.EdgeDirection == direction)
                {
                    layout.DoorConnections.Add(new DoorConnection(from, to, config.FromDoor, config.ToDoor));
                    return true;
                }
            }

            return false;
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
