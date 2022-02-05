using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public static class GraphBranchDecomposer
    {
        /// <summary>
        /// Returns list of branches in the graph originating from the specified trunk nodes.
        /// </summary>
        public static List<List<int>> FindBranches(LayoutGraph graph)
        {
            var cycles = GraphCycleDecomposer.FindCycles(graph);
            var marked = new HashSet<int>(graph.NodeCount());
            var parents = new Dictionary<int, int>(graph.NodeCount());
            var branches = new List<List<int>>();

            // Add trunk nodes to marked set.
            foreach (var cycle in cycles)
            {
                foreach (var node in cycle)
                {
                    marked.Add(node);
                }
            }

            // Search for branches beginning at each trunk node.
            var trunk = marked.ToArray();

            foreach (var node in trunk)
            {
                BranchSearch(graph, node, -1, parents, marked, branches);
            }

            return branches;
        }

        /// <summary>
        /// Performs depth first search for graph branches.
        /// </summary>
        private static void BranchSearch(LayoutGraph graph, int node, int parent, Dictionary<int, int> parents, HashSet<int> marked, List<List<int>> branches)
        {
            // If the node already has a parent, then it has been traversed previously from another trunk node.
            // The node is a member of a connecting branch between two trunk nodes.
            if (parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                AddParentsToBranch(node, branch, parents, marked);
                branches.Add(branch);
                return;
            }

            var neighbors = graph.GetNeighbors(node);
            parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count() == 1)
            {
                var branch = new List<int> { node };
                AddParentsToBranch(node, branch, parents, marked);
                branches.Add(branch);
                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!marked.Contains(neighbor))
                    BranchSearch(graph, neighbor, node, parents, marked, branches);
            }
        }

        /// <summary>
        /// Accumulates the parents of the node into the branch.
        /// </summary>
        private static void AddParentsToBranch(int node, List<int> branch, Dictionary<int, int> parents, HashSet<int> marked)
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
