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
        public Dictionary<int, Room> Rooms { get; } = new();
        public List<DoorConnection> DoorConnections { get; } = new();

        public Layout(int seed)
        {
            Seed = seed;
        }

        public Layout(Layout baseLayout)
        {
            Seed = baseLayout.Seed;
            Rooms = new(baseLayout.Rooms);
            DoorConnections = new(baseLayout.DoorConnections);
        }

        public override string ToString()
        {
            return $"Layout(Seed = {Seed})";
        }

        public static Layout Generate(int seed, LayoutGraph graph, TemplateGroups groups, int maxRebases = 10000, int maxBranchLength = -1)
        {
            int chain = 0;
            var random = new Random(seed);
            var spaces = groups.GetConfigurationSpaces();
            var chains = new GraphChainDecomposer(graph, maxBranchLength).FindChains();
            var rebases = new Stack<int>();
            var layouts = new Stack<Layout>();
            layouts.Push(new(seed));
            rebases.Push(0);

            while (layouts.Count > 0)
            {
                var baseLayout = layouts.Peek();
                var rebase = rebases.Pop();

                if (chain >= chains.Count)
                {
                    return baseLayout;
                }

                if (rebase >= maxRebases)
                {
                    layouts.Pop();
                    chain--;
                    continue;
                }

                rebases.Push(rebase + 1);
                var layout = new Layout(baseLayout);
                
                if (layout.AddChain(chains[chain], graph, groups, spaces, random))
                {
                    rebases.Push(0);
                    layouts.Push(layout);
                    chain++;
                }
            }

            return null;
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

        public bool AddChain(List<LayoutEdge> chain, LayoutGraph graph, TemplateGroups groups,
            Dictionary<TemplatePair, ConfigurationSpace> spaces, Random random)
        {
            foreach (var edge in chain)
            {
                if (!AddEdge(edge, graph, groups, spaces, random))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AddEdge(LayoutEdge edge, LayoutGraph graph, TemplateGroups groups,
            Dictionary<TemplatePair, ConfigurationSpace> spaces, Random random)
        {
            var fromExists = Rooms.TryGetValue(edge.FromNode, out var fromRoom);
            var toExists = Rooms.TryGetValue(edge.ToNode, out var toRoom);

            if (fromExists && toExists)
            {
                return AddDoorConnection(edge, random);
            }

            if (!fromExists && !toExists)
            {
                var fromNode = graph.GetNode(edge.FromNode);
                var toNode = graph.GetNode(edge.ToNode);

                return AddFirstRoom(fromNode, groups, random)
                    && AddRoom(Rooms[edge.FromNode], toNode, edge.Direction, groups, spaces, random);
            }

            if (!fromExists)
            {
                var node = graph.GetNode(edge.FromNode);
                var direction = Door.ReverseEdgeDirection(edge.Direction);
                return AddRoom(toRoom, node, direction, groups, spaces, random);
            }

            if (!toExists)
            {
                var node = graph.GetNode(edge.ToNode);
                return AddRoom(fromRoom, node, edge.Direction, groups, spaces, random);
            }

            return false;
        }

        private bool AddRoom(Room room, LayoutNode node, EdgeDirection direction, TemplateGroups groups,
            Dictionary<TemplatePair, ConfigurationSpace> spaces, Random random)
        {
            var templates = groups.GetTemplates(node.TemplateGroups);
            Statistics.Shuffle(templates, random);
            
            foreach (var template in templates)
            {
                var space = spaces[new(room.Template, template)];
                var configurations = Statistics.Shuffled(space.Configurations, random);

                foreach (var config in configurations)
                {
                    var x = config.X + room.X;
                    var y = config.Y + room.Y;

                    if (config.EdgeDirection == direction && !Intersects(template, x, y))
                    {
                        var newRoom = new Room(node.Id, x, y, template);
                        var connection = new DoorConnection(room, newRoom, config.FromDoor, config.ToDoor);
                        Rooms.Add(node.Id, newRoom);
                        DoorConnections.Add(connection);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AddFirstRoom(LayoutNode node, TemplateGroups templateGroups, Random random)
        {
            var templates = templateGroups.GetTemplates(node.TemplateGroups);
            Statistics.Shuffle(templates, random);

            foreach (var template in templates)
            {
                if (!Intersects(template, 0, 0))
                {
                    Rooms.Add(node.Id, new(node.Id, 0, 0, template));
                    return true;
                }
            }

            return false;
        }

        private bool AddDoorConnection(LayoutEdge edge, Random random)
        {
            var from = Rooms[edge.FromNode];
            var to = Rooms[edge.ToNode];
            var doorPairs = from.Template.AlignedDoors(to.Template, to.X - from.X, to.Y - from.Y);
            Statistics.Shuffle(doorPairs, random);

            foreach (var pair in doorPairs)
            {
                if (pair.EdgeDirection() == edge.Direction)
                {
                    DoorConnections.Add(new(from, to, pair.FromDoor, pair.ToDoor));
                    return true;
                }
            }

            return false;
        }
    }
}
