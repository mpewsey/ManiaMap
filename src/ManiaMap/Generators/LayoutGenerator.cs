using MPewsey.Common.Mathematics;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Graphs;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Generators
{
    /// <summary>
    /// A class for generator room Layout based on various RoomTemplate and a LayoutGraph.
    /// 
    /// The algorithm is based on [1] but does not include a simulated annealing evolver.
    /// 
    /// References
    /// ----------
    /// * [1] Nepožitek, Ondřej. (2019, January 13). Dungeon Generator (Part 2) – Implementation. Retrieved February 8, 2022, from https://ondra.nepozitek.cz/blog/238/dungeon-generator-part-2-implementation/
    /// </summary>
    public class LayoutGenerator : IGenerationStep
    {
        private int _maxRebases;
        /// <summary>
        /// The maximum number of times that a sub layout can be used as a base before it is discarded.
        /// </summary>
        public int MaxRebases { get => _maxRebases; set => _maxRebases = Math.Max(value, 1); }

        private float _rebaseDecayRate;
        /// <summary>
        /// The rebase decay rate. This should be a value greater than or equal to zero.
        /// </summary>
        public float RebaseDecayRate { get => _rebaseDecayRate; set => _rebaseDecayRate = Math.Max(value, 0); }

        /// <summary>
        /// The maximum branch chain length. Branch chains exceeding this length will be split.
        /// Negative and zero values will be ignored.
        /// </summary>
        public int MaxBranchLength { get; set; }

        /// <summary>
        /// The layout graph.
        /// </summary>
        private LayoutGraph Graph { get; set; }

        /// <summary>
        /// The template groups.
        /// </summary>
        private TemplateGroups TemplateGroups { get; set; }

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
        /// The allowable number of layout rebases for the current chain.
        /// </summary>
        private int AllowableRebases { get; set; } = 1;

        private int _chainIndex;
        /// <summary>
        /// The current chain index.
        /// </summary>
        private int ChainIndex
        {
            get => _chainIndex;
            set
            {
                _chainIndex = value;
                SetAllowableRebases(value);
            }
        }

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="maxRebases">The maximum number of times that a sub layout can be used as a base before it is discarded.</param>
        /// <param name="rebaseDecayRate">The rebase decay rate. This should be a value greater than or equal to zero.</param>
        /// <param name="maxBranchLength">The maximum branch chain length. Branch chains exceeding this length will be split. Negative and zero values will be ignored.</param>
        public LayoutGenerator(int maxRebases = 100, float rebaseDecayRate = 0.25f, int maxBranchLength = -1)
        {
            MaxRebases = maxRebases;
            RebaseDecayRate = rebaseDecayRate;
            MaxBranchLength = maxBranchLength;
        }

        public override string ToString()
        {
            return $"LayoutGenerator(MaxRebases = {MaxRebases}, RebaseDecayRate = {RebaseDecayRate}, MaxBranchLength = {MaxBranchLength})";
        }

        /// <summary>
        /// Sets the allowable number of rebases for the specified chain index.
        /// </summary>
        /// <param name="chain">The chain index.</param>
        private void SetAllowableRebases(int chain)
        {
            var value = (int)Math.Ceiling(MaxRebases * Math.Exp(-chain * RebaseDecayRate));
            AllowableRebases = Math.Max(value, 1);
        }

        /// <summary>
        /// Generates a new layout and adds it to the results output dictionary.
        /// 
        /// The following arguments are required:
        /// * %LayoutId - The layout ID.
        /// * %LayoutGraph - The layout graph.
        /// * %TemplateGroups - The template groups.
        /// * %RandomSeed - The random seed.
        /// 
        /// The following entries are added to the results output dictionary:
        /// * %Layout - The generated layout.
        /// </summary>
        /// <param name="results">The pipeline results.</param>
        public bool ApplyStep(GenerationPipeline.Results results)
        {
            var layoutId = results.GetArgument<int>("LayoutId");
            var graph = results.GetArgument<LayoutGraph>("LayoutGraph");
            var templateGroups = results.GetArgument<TemplateGroups>("TemplateGroups");
            var randomSeed = results.GetArgument<RandomSeed>("RandomSeed");

            var layout = Generate(layoutId, graph, templateGroups, randomSeed);
            results.SetOutput("Layout", layout);
            return layout != null;
        }

        /// <summary>
        /// Initializes the generator.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="templateGroups">The template groups.</param>
        /// <param name="randomSeed">The random seed.</param>
        private void Initialize(LayoutGraph graph, TemplateGroups templateGroups, RandomSeed randomSeed)
        {
            graph.Validate();
            templateGroups.Validate();

            Graph = graph;
            TemplateGroups = templateGroups;
            RandomSeed = randomSeed;
            ConfigurationSpaces = templateGroups.GetConfigurationSpaces();
            ChainIndex = 0;
        }

        /// <summary>
        /// Generates and returns a new layout.
        /// </summary>
        /// <param name="layoutId">The layout ID.</param>
        /// <param name="graph">The layout graph.</param>
        /// <param name="templateGroups">The template groups.</param>
        /// <param name="randomSeed">The random seed.</param>
        public Layout Generate(int layoutId, LayoutGraph graph, TemplateGroups templateGroups, RandomSeed randomSeed)
        {
            GenerationLogger.Log("Running layout generator...");
            Initialize(graph, templateGroups, randomSeed);

            var chains = Graph.FindChains(MaxBranchLength);
            var baseLayout = new Layout(layoutId, graph.Name, randomSeed.Seed);
            var layouts = new Stack<Layout>(chains.Count);
            layouts.Push(baseLayout);

            while (layouts.Count > 0)
            {
                Layout = layouts.Peek();

                // If all chains have been added, validate the layout.
                if (ChainIndex >= chains.Count)
                {
                    // If layout is complete, return the layout.
                    if (Layout.IsComplete(TemplateGroups))
                    {
                        Layout = new Layout(Layout);
                        GenerationLogger.Log("Layout generator complete.");
                        return Layout;
                    }

                    // If layout is not complete, start over.
                    ChainIndex = 0;
                    layouts.Clear();
                    layouts.Push(baseLayout);
                    GenerationLogger.Log("Layout constraints not satisfied. Restarting...");
                    continue;
                }

                // If layout has been used as a base more than the maximum allowable count,
                // remove the layout and backtrack.
                if (Layout.Rebases > AllowableRebases)
                {
                    layouts.Pop();
                    ChainIndex--;
                    GenerationLogger.Log("Rebase count exceeded. Backtracking...");
                    continue;
                }

                // Create a new layout and try to add the next chain.
                Layout = new Layout(Layout);

                if (AddChain(chains[ChainIndex]))
                {
                    layouts.Push(Layout);
                    ChainIndex++;
                    GenerationLogger.Log($"Added chain {ChainIndex} / {chains.Count}...");
                }
            }

            GenerationLogger.Log("Layout generator failed.");
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
            return RandomSeed.Shuffled(space.Configurations);
        }

        /// <summary>
        /// Returns a new shuffled list of template group entries for the group.
        /// </summary>
        /// <param name="group">The template group name.</param>
        private List<TemplateGroups.Entry> GetTemplateGroupEntries(string group)
        {
            var entries = TemplateGroups.GetGroup(group);
            var result = new List<TemplateGroups.Entry>(entries.Count);

            foreach (var entry in entries)
            {
                var count = Layout.GetTemplateCount(entry);

                if (count < entry.MaxQuantity)
                    result.Add(entry);
            }

            RandomSeed.Shuffle(result);
            return result;
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
            var addBackEdgeRoom = RandomSeed.ChanceSatisfied(backEdge.RoomChance);
            var addAheadEdgeRoom = RandomSeed.ChanceSatisfied(aheadEdge.RoomChance);
            var addedBackEdgeRoom = addBackEdgeRoom && AddRoom(backEdge, backNode.RoomId, backEdge.DoorCode, backEdge.Direction);
            var addedAheadEdgeRoom = addAheadEdgeRoom && AddRoom(aheadEdge, aheadNode.RoomId, aheadEdge.DoorCode, LayoutEdge.ReverseEdgeDirection(aheadEdge.Direction));

            // If a back edge room is required and was not added, abort.
            if (!addedBackEdgeRoom && addBackEdgeRoom && backEdge.RequireRoom)
                return false;

            // If an ahead edge room is required and was not added, abort.
            if (!addedAheadEdgeRoom && addAheadEdgeRoom && aheadEdge.RequireRoom)
                return false;

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
        /// <exception cref="InvalidChainOrderException">Raised if the to node already exist, indicating the chains are not properly ordered.</exception>
        private bool AddRooms(LayoutEdge edge)
        {
            var fromNode = Graph.GetNode(edge.FromNode);
            var toNode = Graph.GetNode(edge.ToNode);
            var fromRoomExists = Layout.Rooms.ContainsKey(fromNode.RoomId);
            var toRoomExists = Layout.Rooms.ContainsKey(toNode.RoomId);
            var addEdgeRoom = RandomSeed.ChanceSatisfied(edge.RoomChance);

            if (toRoomExists)
                throw new InvalidChainOrderException("To Node Exists. Chains are not properly ordered.");

            // If both rooms do not exist, add the first room. If that fails, abort.
            if (!fromRoomExists && !AddFirstRoom(fromNode))
                return false;

            // Try to add a room for the edge.
            var addedEdgeRoom = addEdgeRoom && AddRoom(edge, fromNode.RoomId, edge.DoorCode, edge.Direction);

            // If an edge room is required and was not added, abort.
            if (!addedEdgeRoom && addEdgeRoom && edge.RequireRoom)
                return false;

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
            foreach (var entry in GetTemplateGroupEntries(source.TemplateGroup))
            {
                var room = new Room(source, Vector2DInt.Zero, entry.Template, RandomSeed);
                Layout.Rooms.Add(room.Id, room);
                Layout.IncreaseTemplateCount(entry);
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

            foreach (var entry in GetTemplateGroupEntries(source.TemplateGroup))
            {
                foreach (var config in GetConfigurations(fromRoom.Template, entry.Template))
                {
                    var position = config.Position + (Vector2DInt)fromRoom.Position;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config.Matches(z, code, direction))
                        continue;
                    if (Layout.Intersects(entry.Template, position, source.Z))
                        continue;

                    // Add the room to the layout.
                    var room = new Room(source, position, entry.Template, RandomSeed);
                    Layout.Rooms.Add(room.Id, room);

                    // Try to create a door connection between the rooms.
                    if (!AddDoorConnection(fromRoomId, source.RoomId, config))
                    {
                        // Remove the room added previously.
                        Layout.Rooms.Remove(room.Id);
                        continue;
                    }

                    Layout.IncreaseTemplateCount(entry);
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

            foreach (var entry in GetTemplateGroupEntries(source.TemplateGroup))
            {
                foreach (var config1 in GetConfigurations(backRoom.Template, entry.Template))
                {
                    var position = config1.Position + (Vector2DInt)backRoom.Position;

                    // Check if the configuration can be added to the layout. If not, try the next one.
                    if (!config1.Matches(z1, backCode, backDirection))
                        continue;
                    if (Layout.Intersects(entry.Template, position, source.Z))
                        continue;

                    foreach (var config2 in GetConfigurations(entry.Template, aheadRoom.Template))
                    {
                        var offset = (Vector2DInt)aheadRoom.Position - position;

                        // Check if the configuration can be added to the layout. If not, try the next one.
                        if (!config2.Matches(offset, z2, aheadCode, aheadDirection))
                            continue;

                        var room = new Room(source, position, entry.Template, RandomSeed);
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

                        Layout.IncreaseTemplateCount(entry);
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
            Box shaft = null;

            if (Math.Abs(fromRoom.Position.Z - toRoom.Position.Z) > 1)
            {
                // Shaft is required.
                var position = fromDoor.Position + (Vector2DInt)fromRoom.Position;
                var zMin = Math.Min(fromRoom.Position.Z, toRoom.Position.Z) + 1;
                var zMax = Math.Max(fromRoom.Position.Z, toRoom.Position.Z) - 1;
                var min = new Vector3DInt(position.X, position.Y, zMin);
                var max = new Vector3DInt(position.X, position.Y, zMax);

                // If shaft intersects the layout, abort adding the door connection.
                if (Layout.Intersects(min, max))
                    return false;

                shaft = new Box(min, max);
            }

            var connection = new DoorConnection(fromRoom, toRoom, fromDoor, toDoor, shaft);
            Layout.DoorConnections.Add(new RoomPair(fromRoomId, toRoomId), connection);
            return true;
        }
    }
}
