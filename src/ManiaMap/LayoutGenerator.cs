﻿using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for generator room `Layout` based on various `RoomTemplate` and a `LayoutGraph`.
    /// 
    /// The algorithm is based on [1] but does not include a simulated annealing evolver.
    /// 
    /// References
    /// ----------
    /// * [1] Nepožitek, Ondřej. (2019, January 13). Dungeon Generator (Part 2) – Implementation. Retrieved February 8, 2022, from https://ondra.nepozitek.cz/blog/238/dungeon-generator-part-2-implementation/
    /// </summary>
    public class LayoutGenerator
    {
        /// <summary>
        /// The layout graph.
        /// </summary>
        public LayoutGraph Graph { get; set; }

        /// <summary>
        /// The template groups.
        /// </summary>
        public TemplateGroups TemplateGroups { get; set; }

        /// <summary>
        /// The generator settings.
        /// </summary>
        public LayoutGeneratorSettings Settings { get; set; }

        /// <summary>
        /// A dictionary of configuration spaces by template pair.
        /// </summary>
        private Dictionary<TemplatePair, ConfigurationSpace> ConfigurationSpaces { get; set; }

        /// <summary>
        /// The random number generator.
        /// </summary>
        private RandomSeed RandomSeed { get; set; }

        /// <summary>
        /// The current layout.
        /// </summary>
        private Layout Layout { get; set; }

        /// <summary>
        /// Initializes a layout generator.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="templateGroups">The template groups.</param>
        /// <param name="settings">The generator settings.</param>
        public LayoutGenerator(LayoutGraph graph, TemplateGroups templateGroups, LayoutGeneratorSettings settings = null)
        {
            Graph = graph;
            TemplateGroups = templateGroups;
            Settings = settings ?? new LayoutGeneratorSettings();
        }

        public override string ToString()
        {
            return $"LayoutGenerator(Graph = {Graph}, TemplateGroups = {TemplateGroups}, Settings = {Settings})";
        }

        /// <summary>
        /// Generates and returns a new layout.
        /// </summary>
        /// <param name="id">The layout ID.</param>
        /// <param name="random">The random seed.</param>
        public Layout GenerateLayout(int id, RandomSeed random)
        {
            RandomSeed = random;
            ConfigurationSpaces = TemplateGroups.GetConfigurationSpaces();

            int chain = 0;
            var chains = Graph.FindChains(Settings.MaxBranchLength);
            var layouts = new Stack<Layout>();
            layouts.Push(new Layout(id, Graph.Name, RandomSeed));

            while (layouts.Count > 0)
            {
                Layout = layouts.Peek();

                // If all chains have been added, return the layout.
                if (chain >= chains.Count)
                    return Layout;

                // If layout has been used as a base more than the maximum allowable count,
                // remove the layout and backtrack.
                if (Layout.Rebases >= Settings.MaxRebases)
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

        /// <summary>
        /// Returns a new shuffled list of configurations for the room templates.
        /// </summary>
        /// <param name="x">The from room template.</param>
        /// <param name="y">The to room template.</param>
        private List<Configuration> GetConfigurations(RoomTemplate x, RoomTemplate y)
        {
            var space = ConfigurationSpaces[new TemplatePair(x, y)];
            var result = new List<Configuration>(space.Configurations);
            RandomSeed.Shuffle(result);
            return result;
        }

        /// <summary>
        /// Returns a new shuffled list of room templates for the groups.
        /// </summary>
        /// <param name="group">The template group name.</param>
        private List<RoomTemplate> GetTemplates(string group)
        {
            var templates = TemplateGroups.GetTemplates(group);
            RandomSeed.Shuffle(templates);
            return templates;
        }

        /// <summary>
        /// Attempts to add the chain to the layout. Returns true if successful.
        /// </summary>
        /// <param name="chain">A list of edges in the chain.</param>
        private bool AddChain(List<LayoutEdge> chain)
        {
            for (int i = 0; i < chain.Count; i++)
            {
                var backEdge = chain[i];
                var aheadEdge = i + 1 < chain.Count ? chain[i + 1] : null;

                if (CanInsertRoom(backEdge, aheadEdge))
                {
                    if (!InsertRooms(backEdge, aheadEdge))
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
        /// <param name="backEdge">The back edge.</param>
        /// <param name="aheadEdge">The ahead edge.</param>
        private bool CanInsertRoom(LayoutEdge backEdge, LayoutEdge aheadEdge)
        {
            return aheadEdge != null
                && backEdge.ToNode == aheadEdge.FromNode
                && Layout.Rooms.ContainsKey(new Uid(aheadEdge.ToNode))
                && Layout.Rooms.ContainsKey(new Uid(backEdge.FromNode))
                && !Layout.Rooms.ContainsKey(new Uid(backEdge.ToNode));
        }

        /// <summary>
        /// Attemps to insert the rooms for the edges into the layout. Returns true if successful.
        /// </summary>
        /// <param name="backEdge">The back edge.</param>
        /// <param name="aheadEdge">The ahead edge.</param>
        private bool InsertRooms(LayoutEdge backEdge, LayoutEdge aheadEdge)
        {
            var midNode = Graph.GetNode(backEdge.ToNode);
            var backNode = Graph.GetNode(backEdge.FromNode);
            var aheadNode = Graph.GetNode(aheadEdge.ToNode);

            // Try to add edge rooms.
            var addBackEdgeRoom = backEdge.RoomChanceSatisfied(RandomSeed.Random.NextDouble());
            var addAheadEdgeRoom = aheadEdge.RoomChanceSatisfied(RandomSeed.Random.NextDouble());
            var addedBackEdgeRoom = addBackEdgeRoom && AddRoom(backEdge, backNode.RoomId, backEdge.DoorCode, backEdge.Direction);
            var addedAheadEdgeRoom = addAheadEdgeRoom && AddRoom(aheadEdge, aheadNode.RoomId, aheadEdge.DoorCode, LayoutEdge.ReverseEdgeDirection(aheadEdge.Direction));

            // Try to insert the node room.
            var backRoomId = addedBackEdgeRoom ? backEdge.RoomId : backNode.RoomId;
            var aheadRoomId = addedAheadEdgeRoom ? aheadEdge.RoomId : aheadNode.RoomId;

            return InsertRoom(midNode,
                backRoomId, backEdge.DoorCode, backEdge.Direction,
                aheadRoomId, aheadEdge.DoorCode, aheadEdge.Direction);
        }

        /// <summary>
        /// Attempts to add the rooms for the edge to the layout. Returns true if successful.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <exception cref="Exception">Raised if the to node already exist, indicating the chains are not properly ordered.</exception>
        private bool AddRooms(LayoutEdge edge)
        {
            var fromNode = Graph.GetNode(edge.FromNode);
            var toNode = Graph.GetNode(edge.ToNode);
            var fromRoomExists = Layout.Rooms.ContainsKey(fromNode.RoomId);
            var toRoomExists = Layout.Rooms.ContainsKey(toNode.RoomId);
            var addEdgeRoom = edge.RoomChanceSatisfied(RandomSeed.Random.NextDouble());

            if (toRoomExists)
                throw new Exception("To Node Exists. Chains are not properly ordered.");

            // If both rooms do not exist, add the first room. If that fails, abort.
            if (!fromRoomExists && !AddFirstRoom(fromNode))
                return false;

            // Try to add a room for the edge.
            var addedEdgeRoom = addEdgeRoom && AddRoom(edge, fromNode.RoomId, edge.DoorCode, edge.Direction);

            // Try to add a room for the to node.
            var fromRoomId = addedEdgeRoom ? edge.RoomId : fromNode.RoomId;
            return AddRoom(toNode, fromRoomId, edge.DoorCode, edge.Direction);
        }

        /// <summary>
        /// Attempts to add the nodes from node to the layout. Returns true if successful.
        /// </summary>
        /// <param name="source">The room source.</param>
        private bool AddFirstRoom(IRoomSource source)
        {
            // Get the first template and add it to the layout.
            foreach (var template in GetTemplates(source.TemplateGroup))
            {
                var room = new Room(source, Vector2DInt.Zero, template, RandomSeed);
                Layout.Rooms.Add(room.Id, room);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to add a new room for the to node of the edge. Returns true if successful.
        /// </summary>
        /// <param name="source">The to room source.</param>
        /// <param name="fromRoomId">The from room ID.</param>
        /// <param name="code">The door code.</param>
        /// <param name="direction">The edge direction.</param>
        private bool AddRoom(IRoomSource source,
            Uid fromRoomId, int code, EdgeDirection direction)
        {
            var fromRoom = Layout.Rooms[fromRoomId];
            var z = source.Z - fromRoom.Position.Z;

            foreach (var template in GetTemplates(source.TemplateGroup))
            {
                foreach (var config in GetConfigurations(fromRoom.Template, template))
                {
                    var position = config.Position + fromRoom.Position;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config.Matches(z, code, direction))
                        continue;
                    if (Layout.Intersects(template, position, source.Z))
                        continue;

                    // Add the room to the layout.
                    var room = new Room(source, position, template, RandomSeed);
                    Layout.Rooms.Add(room.Id, room);

                    // Try to create a door connection between the rooms.
                    if (!AddDoorConnection(fromRoomId, source.RoomId, config))
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
        /// <param name="source">The middle room source.</param>
        /// <param name="backRoomId">The back room ID.</param>
        /// <param name="backCode">The back door code.</param>
        /// <param name="backDirection">The back edge direction.</param>
        /// <param name="aheadRoomId">The ahead room ID.</param>
        /// <param name="aheadCode">The ahead door code.</param>
        /// <param name="aheadDirection">The ahead edge direction.</param>
        private bool InsertRoom(IRoomSource source,
            Uid backRoomId, int backCode, EdgeDirection backDirection,
            Uid aheadRoomId, int aheadCode, EdgeDirection aheadDirection)
        {
            var backRoom = Layout.Rooms[backRoomId];
            var aheadRoom = Layout.Rooms[aheadRoomId];
            var z1 = source.Z - backRoom.Position.Z;
            var z2 = aheadRoom.Position.Z - source.Z;

            foreach (var template in GetTemplates(source.TemplateGroup))
            {
                foreach (var config1 in GetConfigurations(backRoom.Template, template))
                {
                    var position = config1.Position + backRoom.Position;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config1.Matches(z1, backCode, backDirection))
                        continue;
                    if (Layout.Intersects(template, position, source.Z))
                        continue;

                    foreach (var config2 in GetConfigurations(template, aheadRoom.Template))
                    {
                        var offset = aheadRoom.Position - position;

                        // Check if the configuration can be added to the layout. If not, try the next one.
                        if (!config2.Matches(offset, z2, aheadCode, aheadDirection))
                            continue;

                        var room = new Room(source, position, template, RandomSeed);
                        Layout.Rooms.Add(room.Id, room);

                        // Try to create a door connection between the rooms.
                        if (!AddDoorConnection(backRoomId, source.RoomId, config1))
                        {
                            // Remove the room added previously.
                            Layout.Rooms.Remove(room.Id);
                            continue;
                        }

                        // Try to create a door connection between the rooms.
                        if (!AddDoorConnection(source.RoomId, aheadRoomId, config2))
                        {
                            // Remove the room and door connection added previously.
                            Layout.Rooms.Remove(room.Id);
                            Layout.RemoveDoorConnection(backRoomId, source.RoomId);
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
        /// <param name="fromRoomId">The from room ID.</param>
        /// <param name="toRoomId">The to room ID.</param>
        /// <param name="config">The configuration.</param>
        private bool AddDoorConnection(Uid fromRoomId, Uid toRoomId, Configuration config)
        {
            var fromRoom = Layout.Rooms[fromRoomId];
            var toRoom = Layout.Rooms[toRoomId];
            var fromDoor = config.FromDoor;
            var toDoor = config.ToDoor;

            if (Math.Abs(fromRoom.Position.Z - toRoom.Position.Z) <= 1)
            {
                // Rooms do not require shaft connection. Simply add the door connection.
                var connection = new DoorConnection(fromRoom, toRoom, fromDoor, toDoor);
                Layout.DoorConnections.Add(new RoomPair(fromRoomId, toRoomId), connection);
            }
            else
            {
                // Shaft is required.
                var position = fromDoor.Position + fromRoom.Position;
                var zMin = Math.Min(fromRoom.Position.Z, toRoom.Position.Z) + 1;
                var zMax = Math.Max(fromRoom.Position.Z, toRoom.Position.Z) - 1;
                var min = new Vector3DInt(position.X, position.Y, zMin);
                var max = new Vector3DInt(position.X, position.Y, zMax);

                // If shaft intersects the layout, abort adding the door connection.
                if (Layout.Intersects(min, max))
                    return false;

                var shaft = new Box(min, max);
                var connection = new DoorConnection(fromRoom, toRoom, fromDoor, toDoor, shaft);
                Layout.DoorConnections.Add(new RoomPair(fromRoomId, toRoomId), connection);
            }

            return true;
        }
    }
}
