using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for finding cycles in a `LayoutGraph`. The algorithm is based on [1].
    /// 
    /// References
    /// ----------
    /// 
    /// * [1] GeeksforGeeks. (2021, July 2). Print all the cycles in an undirected graph. Retrieved February 8, 2022, from https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/
    /// </summary>
    public class GraphCycleDecomposer
    {
        /// <summary>
        /// A list of cycles in the graph.
        /// </summary>
        private List<List<int>> Cycles { get; } = new List<List<int>>();

        /// <summary>
        /// A dictionary of node parents by node ID.
        /// </summary>
        private Dictionary<int, int> Parents { get; } = new Dictionary<int, int>();

        /// <summary>
        /// A dictionary of node colors by node ID.
        /// </summary>
        private Dictionary<int, int> Colors { get; } = new Dictionary<int, int>();

        /// <summary>
        /// The layout graph.
        /// </summary>
        private LayoutGraph Graph { get; set; }

        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        public List<List<int>> FindCycles(LayoutGraph graph)
        {
            Graph = graph;
            Cycles.Clear();

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in graph.GetNodes())
            {
                Parents.Clear();
                Colors.Clear();
                CycleSearch(node.Id, -1);
            }

            return GetUniqueCycles();
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private void CycleSearch(int node, int parent)
        {
            Colors.TryGetValue(node, out var color);

            // Tree traversal from this node is already complete.
            if (color == 2)
                return;

            // Tree traversal from this node is in progress.
            // Therefore, the tree has looped back around, forming a cycle.
            if (color == 1)
            {
                var current = parent;
                var cycle = new List<int> { current };
                Cycles.Add(cycle);

                // Accumulate parents into cycle until origin node is encountered.
                while (current != node)
                {
                    current = Parents[current];
                    cycle.Add(current);
                }

                return;
            }

            // Change color to indicate search is in progress for node.
            Colors[node] = 1;
            Parents[node] = parent;

            foreach (var neighbor in Graph.GetNeighbors(node))
            {
                if (neighbor != Parents[node])
                    CycleSearch(neighbor, node);
            }

            // Change color to indicate search from node is complete.
            Colors[node] = 2;
        }

        /// <summary>
        /// Returns a new list with all unique cycles in the graph.
        /// </summary>
        private List<List<int>> GetUniqueCycles()
        {
            var sets = new List<HashSet<int>>();
            var result = new List<List<int>>();

            foreach (var cycle in Cycles)
            {
                if (!sets.Any(x => x.SetEquals(cycle)))
                {
                    result.Add(cycle);
                    sets.Add(new HashSet<int>(cycle));
                }
            }

            return result;
        }
    }
}
