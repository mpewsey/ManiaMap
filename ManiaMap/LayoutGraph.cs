using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    public class LayoutGraph
    {
        public int Id { get; }
        private Dictionary<int, LayoutNode> Nodes { get; } = new Dictionary<int, LayoutNode>();
        private Dictionary<EdgeIndexes, LayoutEdge> Edges { get; } = new Dictionary<EdgeIndexes, LayoutEdge>();
        private Dictionary<int, List<int>> Neighbors { get; } = new Dictionary<int, List<int>>();

        public LayoutGraph(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"LayoutGraph(Id = {Id}, Nodes.Count = {Nodes.Count}, Edges.Count = {Edges.Count})";
        }

        /// <summary>
        /// Adds a node to the graph and returns it.
        /// </summary>
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
        /// Adds an edge to the graph and returns it.
        /// </summary>
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
        /// Removes an edge from the graph.
        /// </summary>
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
        public LayoutNode GetNode(int id)
        {
            return Nodes[id];
        }

        /// <summary>
        /// Tries to get the edge in the graph.
        /// </summary>
        public bool TryGetEdge(int node1, int node2, out LayoutEdge edge)
        {
            if (Edges.TryGetValue(new EdgeIndexes(node1, node2), out edge))
                return true;
            if (Edges.TryGetValue(new EdgeIndexes(node2, node1), out edge))
                return true;
            return false;
        }

        /// <summary>
        /// Returns the edge with the specified nodes. The order of the input nodes
        /// does not matter.
        /// </summary>
        public LayoutEdge GetEdge(int node1, int node2)
        {
            if (Edges.TryGetValue(new EdgeIndexes(node2, node1), out var edge))
                return edge;
            return Edges[new EdgeIndexes(node1, node2)];
        }

        /// <summary>
        /// Returns the number of nodes in the graph.
        /// </summary>
        public int NodeCount()
        {
            return Nodes.Count;
        }

        /// <summary>
        /// Returns the number of edges in the graph.
        /// </summary>
        public int EdgeCount()
        {
            return Edges.Count;
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
        public IEnumerable<int> GetNeighbors(int id)
        {
            return Neighbors[id];
        }

        /// <summary>
        /// Returns an enumerable of all node ID's in the graph.
        /// </summary>
        public IEnumerable<int> GetNodeIds()
        {
            return Nodes.Keys;
        }

        /// <summary>
        /// Returns a list of cycles in the graph.
        /// </summary>
        public List<List<int>> FindCycles()
        {
            return new GraphCycleDecomposer(this).FindCycles();
        }

        /// <summary>
        /// Returns a list of branches in the graph.
        /// </summary>
        public List<List<int>> FindBranches()
        {
            return new GraphBranchDecomposer(this).FindBranches();
        }

        /// <summary>
        /// Returns a list of chains in the graph.
        /// </summary>
        public List<List<LayoutEdge>> FindChains(int maxBranchLength = -1)
        {
            return new GraphChainDecomposer(this, maxBranchLength).FindChains();
        }
    }
}
