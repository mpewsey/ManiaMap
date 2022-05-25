using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for decomposing the branches of a graph into chains.
    /// </summary>
    /// <typeparam name="T">The type of the node ID.</typeparam>
    public class GraphBranchDecomposer<T>
    {
        /// <summary>
        /// A dictionary of node neighbors by node ID.
        /// </summary>
        private Dictionary<T, List<T>> Neighbors { get; set; }

        /// <summary>
        /// A dictionary of node parents by node ID.
        /// </summary>
        private Dictionary<T, T> Parents { get; set; }

        /// <summary>
        /// A set of marked nodes.
        /// </summary>
        private HashSet<T> Marked { get; set; }

        /// <summary>
        /// A list of branches.
        /// </summary>
        private List<List<T>> Branches { get; set; }

        /// <summary>
        /// Returns a list of branches originating from the graph's cycles.
        /// </summary>
        /// <param name="neighbors">A dictionary of graph neighbors.</param>
        public List<List<T>> FindBranches(Dictionary<T, List<T>> neighbors)
        {
            Neighbors = neighbors;
            Parents = new Dictionary<T, T>(neighbors.Count);
            Marked = new HashSet<T>();
            Branches = new List<List<T>>();
            MarkTrunk();

            // Search for branches beginning at each trunk node.
            var trunk = Marked.OrderBy(x => x).ToList();

            foreach (var node in trunk)
            {
                BranchSearch(node, node);
            }

            return Branches;
        }

        /// <summary>
        /// Marks the trunk of the graph.
        /// </summary>
        private void MarkTrunk()
        {
            var cycles = new GraphCycleDecomposer<T>().FindCycles(Neighbors);

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
                Marked.Add(MaxNeighborNode());
            }
        }

        /// <summary>
        /// Returns the node ID with the maximum number of neighbors.
        /// </summary>
        /// <exception cref="EmptyGraphException">Raised if the graph does not contain any nodes.</exception>
        public T MaxNeighborNode()
        {
            T maxNode = default;
            int maxNeighbors = -1;

            foreach (var pair in Neighbors.OrderBy(x => x.Key))
            {
                var count = pair.Value.Count;

                if (count > maxNeighbors)
                {
                    maxNode = pair.Key;
                    maxNeighbors = count;
                }
            }

            if (maxNeighbors < 0)
                throw new EmptyGraphException("Graph does not contain any nodes.");

            return maxNode;
        }

        /// <summary>
        /// Performs depth first search for graph branches.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private void BranchSearch(T node, T parent)
        {
            // If the node already has a parent, then it has been traversed previously from another trunk node.
            // The node is a member of a connecting branch between two trunk nodes.
            if (Parents.ContainsKey(node))
            {
                var branch = new List<T> { parent, node };
                AddParentsToBranch(node, branch);
                Branches.Add(branch);
                return;
            }

            var neighbors = Neighbors[node];
            Parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count == 1 && !Marked.Contains(node))
            {
                var branch = new List<T> { node };
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
        private void AddParentsToBranch(T node, List<T> branch)
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
