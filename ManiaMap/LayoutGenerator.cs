using System;
using System.Collections.Generic;
using System.Linq;

namespace ManiaMap
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
            
            foreach (var edge in chain)
            {
                if (!AddEdge(layout, edge))
                {
                    return false;
                }
            }

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

            return AddDoorConnection(layout, edge);
        }

        /// <summary>
        /// Attempts to add the room to the layout.
        /// Returns true if successful.
        /// </summary>
        private bool AddRoom(Layout layout, Room room, LayoutNode node, EdgeDirection direction)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                var space = ConfigurationSpaces[new TemplatePair(room.Template, template)];
                Shuffle(space.Configurations);

                foreach (var config in space.Configurations)
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
        private bool AddDoorConnection(Layout layout, LayoutEdge edge)
        {
            var from = layout.Rooms[edge.FromNode];
            var to = layout.Rooms[edge.ToNode];
            var doorPairs = from.Template.AlignedDoors(to.Template, to.X - from.X, to.Y - from.Y);
            Shuffle(doorPairs);

            foreach (var pair in doorPairs)
            {
                if (pair.EdgeDirection() == edge.Direction)
                {
                    layout.DoorConnections.Add(new DoorConnection(from, to, pair.FromDoor, pair.ToDoor));
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
                var j = Random.Next(i, list.Count - 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
