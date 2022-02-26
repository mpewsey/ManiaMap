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
        private Random Random { get; set; }
        private Layout Layout { get; set; }

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
            ConfigurationSpaces = TemplateGroups.GetConfigurationSpaces();
            var chains = Graph.FindChains(MaxBranchLength);
            var layouts = new Stack<Layout>();
            layouts.Push(new Layout(Graph.Name, Seed));

            while (layouts.Count > 0)
            {
                Layout = layouts.Peek();

                // If all chains have been added, return the layout.
                if (chain >= chains.Count)
                    return Layout;

                // If layout has been used as a base more than the maximum allowable count,
                // remove the layout and backtrack.
                if (Layout.Rebases >= MaxRebases)
                {
                    layouts.Pop();
                    chain--;
                    continue;
                }

                // Create a new layout and try to add the next chain.
                Layout = new Layout(Layout);

                if (AddChain(chains[chain]))
                {
                    layouts.Push(Layout);
                    chain++;
                }
            }

            return null;
        }

        private List<Configuration> GetConfigurations(RoomTemplate x, RoomTemplate y)
        {
            var space = ConfigurationSpaces[new TemplatePair(x, y)];
            var result = new List<Configuration>(space.Configurations);
            Shuffle(result);
            return result;
        }

        private List<RoomTemplate> GetTemplates(LayoutNode node)
        {
            var templates = TemplateGroups.GetTemplates(node.TemplateGroups);
            Shuffle(templates);
            return templates;
        }

        /// <summary>
        /// Attempts to add the chain to the layout. Returns true if successful.
        /// </summary>
        private bool AddChain(List<LayoutEdge> chain)
        {
            for (int i = 0; i < chain.Count; i++)
            {
                var backEdge = chain[i];
                var aheadEdge = i + 1 < chain.Count ? chain[i + 1] : null;

                if (CanInsertRoom(backEdge, aheadEdge))
                {
                    if (!InsertRoom(backEdge, aheadEdge))
                        return false;
                    i++;
                }
                else if (!AddRooms(backEdge))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the middle node for the edges can be inserted between existing rooms.
        /// </summary>
        private bool CanInsertRoom(LayoutEdge backEdge, LayoutEdge aheadEdge)
        {
            return aheadEdge != null
                && backEdge.ToNode == aheadEdge.FromNode
                && Layout.Rooms.ContainsKey(new Uid(backEdge.FromNode))
                && Layout.Rooms.ContainsKey(new Uid(aheadEdge.ToNode))
                && !Layout.Rooms.ContainsKey(new Uid(backEdge.ToNode));
        }

        /// <summary>
        /// Attempts to add the rooms for the edge to the layout. Returns true if successful.
        /// </summary>
        private bool AddRooms(LayoutEdge edge)
        {
            var fromRoomExists = Layout.Rooms.ContainsKey(new Uid(edge.FromNode));
            var toRoomExists = Layout.Rooms.ContainsKey(new Uid(edge.ToNode));

            if (!fromRoomExists && !toRoomExists)
                return AddFirstRoom(edge) && AddToRoom(edge);

            if (!fromRoomExists)
                throw new Exception("Chain is not properly ordered.");

            if (!toRoomExists)
                return AddToRoom(edge);

            throw new Exception("Chains are not properly ordered.");
        }

        /// <summary>
        /// Attempts to add the nodes from node to the layout. Returns true if successful.
        /// </summary>
        private bool AddFirstRoom(LayoutEdge edge)
        {
            var node = Graph.GetNode(edge.FromNode);

            // Get the first template and add it to the layout.
            foreach (var template in GetTemplates(node))
            {
                var room = new Room(node, 0, 0, Random.Next(), template);
                Layout.Rooms.Add(room.Id, room);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new room for the to node of the edge. Returns true if successful.
        /// </summary>
        private bool AddToRoom(LayoutEdge edge)
        {
            var fromRoom = Layout.Rooms[new Uid(edge.FromNode)];
            var node = Graph.GetNode(edge.ToNode);
            var z = node.Z - fromRoom.Z;

            foreach (var template in GetTemplates(node))
            {
                foreach (var config in GetConfigurations(fromRoom.Template, template))
                {
                    var x = config.X + fromRoom.X;
                    var y = config.Y + fromRoom.Y;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config.Matches(z, edge) || Layout.Intersects(template, x, y, node.Z))
                        continue;

                    // Add the room to the layout.
                    var room = new Room(node, x, y, Random.Next(), template);
                    Layout.Rooms.Add(room.Id, room);

                    // Try to create a door connection between the rooms.
                    if (!AddDoorConnection(edge, config))
                    {
                        // Remove the room added previously.
                        Layout.Rooms.Remove(room.Id);
                        continue;
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attemps to insert a new room between two rooms. Returns true if successful.
        /// </summary>
        private bool InsertRoom(LayoutEdge backEdge, LayoutEdge aheadEdge)
        {
            var backRoom = Layout.Rooms[new Uid(backEdge.FromNode)];
            var aheadRoom = Layout.Rooms[new Uid(aheadEdge.ToNode)];
            var node = Graph.GetNode(backEdge.ToNode);
            var z1 = node.Z - backRoom.Z;
            var z2 = aheadRoom.Z - node.Z;

            foreach (var template in GetTemplates(node))
            {
                foreach (var config1 in GetConfigurations(backRoom.Template, template))
                {
                    var x1 = config1.X + backRoom.X;
                    var y1 = config1.Y + backRoom.Y;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config1.Matches(z1, backEdge) || Layout.Intersects(template, x1, y1, node.Z))
                        continue;

                    foreach (var config2 in GetConfigurations(template, aheadRoom.Template))
                    {
                        var x2 = aheadRoom.X - x1;
                        var y2 = aheadRoom.Y - y1;

                        // Check if the configuration can be added to the layout. If not, try the next one.
                        if (!config2.Matches(x2, y2, z2, aheadEdge))
                            continue;

                        var room = new Room(node, x1, y1, Random.Next(), template);
                        Layout.Rooms.Add(room.Id, room);

                        // Try to create a door connection between the rooms.
                        if (!AddDoorConnection(backEdge, config1))
                        {
                            // Remove the room added previously.
                            Layout.Rooms.Remove(room.Id);
                            continue;
                        }

                        // Try to create a door connection between the rooms.
                        if (!AddDoorConnection(aheadEdge, config2))
                        {
                            // Remove the room and door connection added previously.
                            Layout.Rooms.Remove(room.Id);
                            Layout.DoorConnections.RemoveAt(Layout.DoorConnections.Count - 1);
                            continue;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attemps to add the door connection for the edge and configuration.
        /// Returns true if successful.
        /// </summary>
        private bool AddDoorConnection(LayoutEdge edge, Configuration config)
        {
            var fromRoom = Layout.Rooms[new Uid(edge.FromNode)];
            var toRoom = Layout.Rooms[new Uid(edge.ToNode)];
            var fromDoor = config.FromDoor;
            var toDoor = config.ToDoor;

            if (Math.Abs(fromRoom.Z - toRoom.Z) <= 1)
            {
                // Rooms do not require shaft connection. Simply add the door connection.
                var connection = new DoorConnection(fromRoom, toRoom, fromDoor, toDoor);
                Layout.DoorConnections.Add(connection);
            }
            else
            {
                // Shaft is required.
                var x = fromDoor.X + fromRoom.X;
                var y = fromDoor.Y + fromRoom.Y;
                var zMin = Math.Min(fromRoom.Z, toRoom.Z) + 1;
                var zMax = Math.Max(fromRoom.Z, toRoom.Z) - 1;

                // If shaft intersects the layout, abort adding the door connection.
                if (Layout.Intersects(x, x, y, y, zMin, zMax))
                    return false;

                var shaft = new Box(x, x, y, y, zMin, zMax);
                var connection = new DoorConnection(fromRoom, toRoom, fromDoor, toDoor, shaft);
                Layout.DoorConnections.Add(connection);
            }

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
