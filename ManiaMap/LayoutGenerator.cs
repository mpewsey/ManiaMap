using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LayoutGenerator(int seed, LayoutGraph graph, TemplateGroups templateGroups, int maxRebases = 10000, int maxBranchLength = 3)
        {
            Seed = seed;
            MaxRebases = maxRebases;
            MaxBranchLength = maxBranchLength;
            Graph = graph;
            TemplateGroups = templateGroups;
        }

        public Layout GenerateLayout()
        {
            int chain = 0;
            Random = new Random(Seed);
            Chains = Graph.FindChains(MaxBranchLength);
            ConfigurationSpaces = TemplateGroups.GetConfigurationSpaces();
            var layouts = new Stack<Layout>();
            layouts.Push(new(Seed, MaxRebases, MaxBranchLength));

            while (layouts.Count > 0)
            {
                var baseLayout = layouts.Peek();

                if (chain >= Chains.Count)
                {
                    return baseLayout;
                }

                if (baseLayout.Rebases >= MaxRebases)
                {
                    layouts.Pop();
                    chain--;
                    continue;
                }

                var layout = new Layout(baseLayout);

                if (AddChain(layout, chain))
                {
                    layouts.Push(layout);
                    chain++;
                }
            }

            return null;
        }

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

        private bool AddRoom(Layout layout, Room room, LayoutNode node, EdgeDirection direction)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                var space = ConfigurationSpaces[new(room.Template, template)];
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

        private bool AddFirstRoom(Layout layout, LayoutNode node)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);

            foreach (var template in templates)
            {
                if (!layout.Intersects(template, 0, 0))
                {
                    layout.Rooms.Add(node.Id, new(node.Id, 0, 0, template));
                    return true;
                }
            }

            return false;
        }

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
                    layout.DoorConnections.Add(new(from, to, pair.FromDoor, pair.ToDoor));
                    return true;
                }
            }

            return false;
        }

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
