using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for decomposing the branches of a `LayoutGraph` into chains.
    /// </summary>
    public class GraphBranchDecomposer
    {
        /// <summary>
        /// The layout graph.
        /// </summary>
        private LayoutGraph Graph { get; set; }

        /// <summary>
        /// A dictionary of node parents by node ID.
        /// </summary>
        private Dictionary<int, int> Parents { get; set; }

        /// <summary>
        /// A set of marked nodes.
        /// </summary>
        private HashSet<int> Marked { get; set; }

        /// <summary>
        /// A list of branches.
        /// </summary>
        private List<List<int>> Branches { get; set; }

        /// <summary>
        /// Returns a list of branches originating from the graph's cycles.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        public List<List<int>> FindBranches(LayoutGraph graph)
        {
            Graph = graph;
            Parents = new Dictionary<int, int>(graph.NodeCount);
            Marked = new HashSet<int>();
            Branches = new List<List<int>>();
            MarkTrunk();

            // Search for branches beginning at each trunk node.
            var trunk = Marked.ToList();
            trunk.Sort();

            foreach (var node in trunk)
            {
                BranchSearch(node, -1);
            }

            return Branches;
        }

        /// <summary>
        /// Marks the trunk of the graph.
        /// </summary>
        private void MarkTrunk()
        {
            var cycles = Graph.FindCycles();

            // Add trunk nodes to marked set.
            foreach (var cycle in cycles)
            {
                foreach (var node in cycle)
                {
                    Marked.Add(node);
                }
            }

            // Use the node with the max neighbors if cycles do not exist.
            if (Marked.Count == 0)
            {
                Marked.Add(Graph.MaxNeighborNode());
            }
        }

        /// <summary>
        /// Performs depth first search for graph branches.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private void BranchSearch(int node, int parent)
        {
            // If the node already has a parent, then it has been traversed previously from another trunk node.
            // The node is a member of a connecting branch between two trunk nodes.
            if (Parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                AddParentsToBranch(node, branch);
                Branches.Add(branch);
                return;
            }

            var neighbors = Graph.GetNeighbors(node);
            Parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count == 1 && !Marked.Contains(node))
            {
                var branch = new List<int> { node };
                AddParentsToBranch(node, branch);
                Branches.Add(branch);
                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!Marked.Contains(neighbor))
                    BranchSearch(neighbor, node);
            }
        }

        /// <summary>
        /// Accumulates the parents of the node into the branch.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="branch">The branch to which nodes will be added.</param>
        private void AddParentsToBranch(int node, List<int> branch)
        {
            var current = node;
            var isMarked = false;
            Marked.Add(node);

            // Accumulate parents into the branch until a marked node is encountered.
            while (!isMarked)
            {
                current = Parents[current];
                branch.Add(current);
                isMarked = !Marked.Add(current);
            }
        }
    }
}
