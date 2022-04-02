using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for finding cycles in a graph. The algorithm is based on [1].
    /// 
    /// References
    /// ----------
    /// 
    /// * [1] GeeksforGeeks. (2021, July 2). Print all the cycles in an undirected graph. Retrieved February 8, 2022, from https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/
    /// </summary>
    /// <typeparam name="T">The type of the node ID.</typeparam>
    public class GraphCycleDecomposer<T>
    {
        /// <summary>
        /// A dictionary of node neighbors by node ID.
        /// </summary>
        private Dictionary<T, List<T>> Neighbors { get; set; }

        /// <summary>
        /// A list of cycles in the graph.
        /// </summary>
        private List<List<T>> Cycles { get; set; }

        /// <summary>
        /// A dictionary of node parents by node ID.
        /// </summary>
        private Dictionary<T, T> Parents { get; set; }

        /// <summary>
        /// A dictionary of node colors by node ID.
        /// </summary>
        private Dictionary<T, int> Colors { get; set; }

        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        /// <param name="neighbors">A dictionary of graph neighbors.</param>
        public List<List<T>> FindCycles(Dictionary<T, List<T>> neighbors)
        {
            Neighbors = neighbors;
            Cycles = new List<List<T>>();
            Parents = new Dictionary<T, T>(neighbors.Count);
            Colors = new Dictionary<T, int>(neighbors.Count);

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in neighbors.Keys.OrderBy(x => x))
            {
                Parents.Clear();
                Colors.Clear();
                CycleSearch(node, node);
            }

            return GetUniqueCycles();
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private void CycleSearch(T node, T parent)
        {
            var comparer = EqualityComparer<T>.Default;
            Colors.TryGetValue(node, out var color);

            // Tree traversal from this node is already complete.
            if (color == 2)
                return;

            // Tree traversal from this node is in progress.
            // Therefore, the tree has looped back around, forming a cycle.
            if (color == 1)
            {
                var current = parent;
                var cycle = new List<T> { current };
                Cycles.Add(cycle);

                // Accumulate parents into cycle until origin node is encountered.
                while (!comparer.Equals(current, node))
                {
                    current = Parents[current];
                    cycle.Add(current);
                }

                return;
            }

            // Change color to indicate search is in progress for node.
            Colors[node] = 1;
            Parents[node] = parent;

            foreach (var neighbor in Neighbors[node])
            {
                if (!comparer.Equals(neighbor, Parents[node]))
                    CycleSearch(neighbor, node);
            }

            // Change color to indicate search from node is complete.
            Colors[node] = 2;
        }

        /// <summary>
        /// Returns a new list with all unique cycles in the graph.
        /// </summary>
        private List<List<T>> GetUniqueCycles()
        {
            var sets = new List<HashSet<T>>();
            var result = new List<List<T>>();

            foreach (var cycle in Cycles)
            {
                if (!sets.Any(x => x.SetEquals(cycle)))
                {
                    result.Add(cycle);
                    sets.Add(new HashSet<T>(cycle));
                }
            }

            return result;
        }
    }
}
