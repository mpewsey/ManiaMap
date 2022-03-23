using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for decomposing the branches of a `LayoutGraph` into chains.
    /// </summary>
    public static class GraphBranchDecomposer
    {
        /// <summary>
        /// Returns a list of branches originating from the graph's cycles.
        /// </summary>
        public static List<List<int>> FindBranches(LayoutGraph graph)
        {
            var marked = new HashSet<int>();
            var parents = new Dictionary<int, int>(graph.NodeCount);
            var branches = new List<List<int>>();
            var cycles = graph.FindCycles();

            // Add trunk nodes to marked set.
            foreach (var cycle in cycles)
            {
                foreach (var node in cycle)
                {
                    marked.Add(node);
                }
            }

            // Use the node with the max neighbors if cycles do not exist.
            if (marked.Count == 0)
            {
                marked.Add(MaxNeighborNode(graph));
            }

            // Search for branches beginning at each trunk node.
            var trunk = marked.ToArray();
            Array.Sort(trunk);

            foreach (var node in trunk)
            {
                BranchSearch(node, -1, graph, branches, marked, parents);
            }

            return branches;
        }

        /// <summary>
        /// Returns the node with the maximum number of neighbors.
        /// </summary>
        private static int MaxNeighborNode(LayoutGraph graph)
        {
            int maxNode = -1;
            int maxNeighbors = -1;

            foreach (var node in graph.GetNodes())
            {
                var count = graph.GetNeighbors(node.Id).Count();

                if (count > maxNeighbors)
                {
                    maxNode = node.Id;
                    maxNeighbors = count;
                }
            }

            return maxNode;
        }

        /// <summary>
        /// Performs depth first search for graph branches.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        /// <param name="graph">The layout graph.</param>
        /// <param name="branches">A list of branch chains.</param>
        /// <param name="marked">A set of marked IDs.</param>
        /// <param name="parents">A dictionary with node parent ID by node ID.</param>
        private static void BranchSearch(int node, int parent,
            LayoutGraph graph, List<List<int>> branches, HashSet<int> marked, Dictionary<int, int> parents)
        {
            // If the node already has a parent, then it has been traversed previously from another trunk node.
            // The node is a member of a connecting branch between two trunk nodes.
            if (parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                AddParentsToBranch(node, branch, marked, parents);
                branches.Add(branch);
                return;
            }

            var neighbors = graph.GetNeighbors(node);
            parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count() == 1 && !marked.Contains(node))
            {
                var branch = new List<int> { node };
                AddParentsToBranch(node, branch, marked, parents);
                branches.Add(branch);
                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!marked.Contains(neighbor))
                    BranchSearch(neighbor, node, graph, branches, marked, parents);
            }
        }

        /// <summary>
        /// Accumulates the parents of the node into the branch.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="branch">The branch to which nodes will be added.</param>
        /// <param name="marked">A set of marked IDs.</param>
        /// <param name="parents">A dictionary with node parent ID by node ID.</param>
        private static void AddParentsToBranch(int node, List<int> branch,
            HashSet<int> marked, Dictionary<int, int> parents)
        {
            var current = node;
            var isMarked = false;
            marked.Add(node);

            // Accumulate parents into the branch until a marked node is encountered.
            while (!isMarked)
            {
                current = parents[current];
                branch.Add(current);
                isMarked = !marked.Add(current);
            }
        }
    }
}
