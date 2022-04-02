using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a graph consisting of `LayoutNode` and `LayoutEdge`.
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

        public override string ToString()
        {
            return $"LayoutGraph(Id = {Id}, Name = {Name})";
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
