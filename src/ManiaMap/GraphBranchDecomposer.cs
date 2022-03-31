using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for decomposing the branches of a `LayoutGraph` into chains.
    /// </summary>
    public static class GraphBranchDecomposer
    {
        /// <summary>
        /// A construct for storing decomposer data.
        /// </summary>
        private class Data
        {
            /// <summary>
            /// The layout graph.
            /// </summary>
            public LayoutGraph Graph { get; }

            /// <summary>
            /// A dictionary of node parents by node ID.
            /// </summary>
            public Dictionary<int, int> Parents { get; }

            /// <summary>
            /// A set of marked nodes.
            /// </summary>
            public HashSet<int> Marked { get; } = new HashSet<int>();

            /// <summary>
            /// A list of branches.
            /// </summary>
            public List<List<int>> Branches { get; } = new List<List<int>>();

            /// <summary>
            /// Initializes the data.
            /// </summary>
            /// <param name="graph">The layout graph.</param>
            public Data(LayoutGraph graph)
            {
                Graph = graph;
                Parents = new Dictionary<int, int>(graph.NodeCount);
            }
        }

        /// <summary>
        /// Returns a list of branches originating from the graph's cycles.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        public static List<List<int>> FindBranches(LayoutGraph graph)
        {
            var data = new Data(graph);
            MarkTrunk(data);

            // Search for branches beginning at each trunk node.
            var trunk = data.Marked.ToList();
            trunk.Sort();

            foreach (var node in trunk)
            {
                BranchSearch(data, node, -1);
            }

            return data.Branches;
        }

        /// <summary>
        /// Marks the trunk of the graph.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void MarkTrunk(Data data)
        {
            var cycles = data.Graph.FindCycles();

            // Add trunk nodes to marked set.
            foreach (var cycle in cycles)
            {
                foreach (var node in cycle)
                {
                    data.Marked.Add(node);
                }
            }

            // Use the node with the max neighbors if cycles do not exist.
            if (data.Marked.Count == 0)
            {
                data.Marked.Add(data.Graph.MaxNeighborNode());
            }
        }

        /// <summary>
        /// Performs depth first search for graph branches.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private static void BranchSearch(Data data, int node, int parent)
        {
            // If the node already has a parent, then it has been traversed previously from another trunk node.
            // The node is a member of a connecting branch between two trunk nodes.
            if (data.Parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                AddParentsToBranch(data, node, branch);
                data.Branches.Add(branch);
                return;
            }

            var neighbors = data.Graph.GetNeighbors(node);
            data.Parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count == 1 && !data.Marked.Contains(node))
            {
                var branch = new List<int> { node };
                AddParentsToBranch(data, node, branch);
                data.Branches.Add(branch);
                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!data.Marked.Contains(neighbor))
                    BranchSearch(data, neighbor, node);
            }
        }

        /// <summary>
        /// Accumulates the parents of the node into the branch.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        /// <param name="node">The node ID.</param>
        /// <param name="branch">The branch to which nodes will be added.</param>
        private static void AddParentsToBranch(Data data, int node, List<int> branch)
        {
            var current = node;
            var isMarked = false;
            data.Marked.Add(node);

            // Accumulate parents into the branch until a marked node is encountered.
            while (!isMarked)
            {
                current = data.Parents[current];
                branch.Add(current);
                isMarked = !data.Marked.Add(current);
            }
        }
    }
}
