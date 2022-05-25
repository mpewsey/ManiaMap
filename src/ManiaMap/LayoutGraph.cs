using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a graph consisting of LayoutNode and LayoutEdge.
    /// </summary>
    [DataContract]
    public class LayoutGraph
    {
        /// <summary>
        /// The graph ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; private set; }

        /// <summary>
        /// The graph name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A dictionary of nodes by ID.
        /// </summary>
        [DataMember(Order = 3)]
        private Dictionary<int, LayoutNode> Nodes { get; set; } = new Dictionary<int, LayoutNode>();

        /// <summary>
        /// A dictionary of nodes by from and to node ID's.
        /// </summary>
        [DataMember(Order = 4)]
        private Dictionary<EdgeIndexes, LayoutEdge> Edges { get; set; } = new Dictionary<EdgeIndexes, LayoutEdge>();

        /// <summary>
        /// A dictionary of neighboring nodes by node ID.
        /// </summary>
        [DataMember(Order = 5)]
        private Dictionary<int, List<int>> Neighbors { get; set; } = new Dictionary<int, List<int>>();

        /// <summary>
        /// A dictionary of node variation groups.
        /// </summary>
        [DataMember(Order = 6)]
        private Dictionary<string, List<int>> NodeVariations { get; set; } = new Dictionary<string, List<int>>();

        /// <summary>
        /// The number of nodes in the graph.
        /// </summary>
        public int NodeCount => Nodes.Count;

        /// <summary>
        /// The number of edges in the graph.
        /// </summary>
        public int EdgeCount => Edges.Count;

        /// <summary>
        /// Initializes a graph.
        /// </summary>
        /// <param name="id">The unique ID.</param>
        /// <param name="name">The graph name.</param>
        public LayoutGraph(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Initializes a copy of the graph.
        /// </summary>
        /// <param name="other">The graph to be copied.</param>
        private LayoutGraph(LayoutGraph other)
        {
            Id = other.Id;
            Name = other.Name;
            Nodes = other.Nodes.ToDictionary(x => x.Key, x => x.Value.Copy());
            Edges = other.Edges.ToDictionary(x => x.Key, x => x.Value.Copy());
            Neighbors = other.Neighbors.ToDictionary(x => x.Key, x => new List<int>(x.Value));
            NodeVariations = other.NodeVariations.ToDictionary(x => x.Key, x => new List<int>(x.Value));
        }

        public override string ToString()
        {
            return $"LayoutGraph(Id = {Id}, Name = {Name})";
        }

        /// <summary>
        /// Returns a copy of the graph.
        /// </summary>
        public LayoutGraph Copy()
        {
            return new LayoutGraph(this);
        }

        /// <summary>
        /// Returns a readonly list of the node variations for the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        public IReadOnlyList<int> GetNodeVariations(string group)
        {
            return NodeVariations[group];
        }

        /// <summary>
        /// Returns a new variation of the graph.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        public LayoutGraph GetVariation(RandomSeed randomSeed)
        {
            var graph = Copy();
            graph.ApplyVariations(randomSeed);
            return graph;
        }

        /// <summary>
        /// Applies random variations to the graph.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        private void ApplyVariations(RandomSeed randomSeed)
        {
            foreach (var pair in NodeVariations.OrderBy(x => x.Key))
            {
                var variations = randomSeed.Shuffled(pair.Value);

                for (int i = 0; i < variations.Count; i++)
                {
                    SwapEdges(pair.Value[i], variations[i]);
                }
            }
        }

        /// <summary>
        /// Returns the node variations list for the group. If the group does not already exist,
        /// creates it and returns a new list.
        /// </summary>
        /// <param name="group">The group name.</param>
        private List<int> FetchNodeVariations(string group)
        {
            if (!NodeVariations.TryGetValue(group, out var list))
            {
                list = new List<int>();
                NodeVariations.Add(group, list);
            }

            return list;
        }

        /// <summary>
        /// Adds a node variation to the graph. If the node does not already exist, creates it.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="id">The node ID.</param>
        public void AddNodeVariation(string group, int id)
        {
            AddNode(id);
            var variations = FetchNodeVariations(group);

            if (!variations.Contains(id))
                variations.Add(id);
        }

        /// <summary>
        /// Adds node variations to the graph. If the nodes do not already exist, creates them.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="ids">The node ID's.</param>
        public void AddNodeVariation(string group, IEnumerable<int> ids)
        {
            var variations = FetchNodeVariations(group);

            foreach (var id in ids)
            {
                AddNode(id);

                if (!variations.Contains(id))
                    variations.Add(id);
            }
        }

        /// <summary>
        /// Removes all variations for the node.
        /// </summary>
        /// <param name="id">he node ID.</param>
        public void RemoveNodeVariations(int id)
        {
            foreach (var variations in NodeVariations.Values)
            {
                variations.Remove(id);
            }
        }

        /// <summary>
        /// Removes the node variation from the graph.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="id">The node ID.</param>
        public void RemoveNodeVariation(string group, int id)
        {
            FetchNodeVariations(group).Remove(id);
        }

        /// <summary>
        /// Removes the node variations from the graph.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="ids">The node ID.</param>
        public void RemoveNodeVariation(string group, IEnumerable<int> ids)
        {
            var variations = FetchNodeVariations(group);

            foreach (var id in ids)
            {
                variations.Remove(id);
            }
        }

        /// <summary>
        /// Returns a enumerable of edges with the node ID.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public IEnumerable<LayoutEdge> GetEdges(int id)
        {
            return Neighbors[id].Select(x => GetEdge(id, x));
        }

        /// <summary>
        /// Swaps the edges for two nodes in the graph.
        /// </summary>
        /// <param name="id1">The first node ID.</param>
        /// <param name="id2">The second node ID.</param>
        public void SwapEdges(int id1, int id2)
        {
            if (id1 == id2)
                return;

            // Get a set of all edges with the nodes.
            var edges = GetEdges(id1).Concat(GetEdges(id2)).Distinct().ToList();

            // Remove existing edges
            foreach (var edge in edges)
            {
                RemoveEdge(edge.FromNode, edge.ToNode);
            }

            // Create new edges with the node replacements and copy properties to new edges.
            foreach (var edge in edges)
            {
                var node1 = edge.FromNode == id1 ? id2 : edge.FromNode == id2 ? id1 : edge.FromNode;
                var node2 = edge.ToNode == id1 ? id2 : edge.ToNode == id2 ? id1 : edge.ToNode;
                AddEdge(node1, node2).SetProperties(edge);
            }
        }

        /// <summary>
        /// Adds a node to the graph and returns it. If the node already exists, returns
        /// the existing node.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public LayoutNode AddNode(int id)
        {
            if (!Nodes.TryGetValue(id, out var node))
            {
                node = new LayoutNode(id);
                Nodes.Add(id, node);
            }

            if (!Neighbors.ContainsKey(id))
            {
                Neighbors.Add(id, new List<int>());
            }

            return node;
        }

        /// <summary>
        /// Adds an edge to the graph and returns it. If the edge already exists,
        /// returns the existing edge. If the nodes referenced by the edge do not
        /// already exist, new nodes are added to the graph.
        /// </summary>
        /// <param name="fromNode">The from node ID.</param>
        /// <param name="toNode">The to node ID.</param>
        public LayoutEdge AddEdge(int fromNode, int toNode)
        {
            if (fromNode == toNode)
                throw new InvalidIdException($"From node cannot be the same as to node: {fromNode}.");

            if (!TryGetEdge(fromNode, toNode, out var edge))
            {
                edge = new LayoutEdge(fromNode, toNode);
                Edges.Add(new EdgeIndexes(fromNode, toNode), edge);
            }

            AddNode(fromNode);
            AddNode(toNode);

            var fromNeighbors = Neighbors[fromNode];
            var toNeighbors = Neighbors[toNode];

            if (!fromNeighbors.Contains(toNode))
                fromNeighbors.Add(toNode);

            if (!toNeighbors.Contains(fromNode))
                toNeighbors.Add(fromNode);

            return edge;
        }

        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public void RemoveNode(int id)
        {
            var neighbors = Neighbors[id];

            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                var neighbor = neighbors[i];
                Neighbors[neighbor].Remove(id);

                if (!Edges.Remove(new EdgeIndexes(id, neighbor)))
                    Edges.Remove(new EdgeIndexes(neighbor, id));
            }

            Neighbors.Remove(id);
            Nodes.Remove(id);
            RemoveNodeVariations(id);
        }

        /// <summary>
        /// Removes an edge from the graph. The order of the specified node ID's
        /// does not matter.
        /// </summary>
        /// <param name="node1">The first node ID.</param>
        /// <param name="node2">The second node ID.</param>
        public void RemoveEdge(int node1, int node2)
        {
            Neighbors[node1].Remove(node2);
            Neighbors[node2].Remove(node1);

            if (!Edges.Remove(new EdgeIndexes(node1, node2)))
                Edges.Remove(new EdgeIndexes(node2, node1));
        }

        /// <summary>
        /// Returns the node in the graph.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public LayoutNode GetNode(int id)
        {
            return Nodes[id];
        }

        /// <summary>
        /// Tries to get the edge in the graph. Returns true if successful.
        /// The order of the specified node ID's does not matter.
        /// </summary>
        /// <param name="node1">The first node ID.</param>
        /// <param name="node2">The second node ID.</param>
        /// <param name="edge">The returned edge.</param>
        public bool TryGetEdge(int node1, int node2, out LayoutEdge edge)
        {
            return Edges.TryGetValue(new EdgeIndexes(node1, node2), out edge)
                || Edges.TryGetValue(new EdgeIndexes(node2, node1), out edge);
        }

        /// <summary>
        /// Returns the edge with the specified nodes. The order of the input nodes
        /// does not matter.
        /// </summary>
        /// <param name="node1">The first node ID.</param>
        /// <param name="node2">The second node ID.</param>
        public LayoutEdge GetEdge(int node1, int node2)
        {
            if (Edges.TryGetValue(new EdgeIndexes(node2, node1), out var edge))
                return edge;
            return Edges[new EdgeIndexes(node1, node2)];
        }

        /// <summary>
        /// Returns an enumerable of all nodes in the graph.
        /// </summary>
        public IEnumerable<LayoutNode> GetNodes()
        {
            return Nodes.Values;
        }

        /// <summary>
        /// Returns an enumerable of all edges in the graph.
        /// </summary>
        public IEnumerable<LayoutEdge> GetEdges()
        {
            return Edges.Values;
        }

        /// <summary>
        /// Returns an enumerable of all neighbors for the specified node.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public IReadOnlyList<int> GetNeighbors(int id)
        {
            return Neighbors[id];
        }

        /// <summary>
        /// Returns a list of cycles in the graph.
        /// </summary>
        public List<List<int>> FindCycles()
        {
            return new GraphCycleDecomposer<int>().FindCycles(Neighbors);
        }

        /// <summary>
        /// Returns a list of branches in the graph.
        /// </summary>
        public List<List<int>> FindBranches()
        {
            return new GraphBranchDecomposer<int>().FindBranches(Neighbors);
        }

        /// <summary>
        /// Returns a list of chains in the graph.
        /// </summary>
        /// <param name="maxBranchLength">The maximum branch chain length. Branch chains exceeding this length will be split. Negative and zero values will be ignored.</param>
        public List<List<LayoutEdge>> FindChains(int maxBranchLength = -1)
        {
            return new GraphChainDecomposer().FindChains(this, maxBranchLength);
        }
    }
}
