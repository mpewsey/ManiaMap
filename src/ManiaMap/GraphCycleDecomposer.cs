using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for finding cycles in a `LayoutGraph`. The algorithm is based on [1].
    /// 
    /// References
    /// ----------
    /// 
    /// * [1] GeeksforGeeks. (2021, July 2). Print all the cycles in an undirected graph. Retrieved February 8, 2022, from https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/
    /// </summary>
    public static class GraphCycleDecomposer
    {
        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        public static List<List<int>> FindCycles(LayoutGraph graph)
        {
            var cycles = new List<List<int>>();
            var parents = new Dictionary<int, int>(graph.NodeCount);
            var colors = new Dictionary<int, int>(graph.NodeCount);

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in graph.GetNodes())
            {
                parents.Clear();
                colors.Clear();
                CycleSearch(node.Id, -1, graph, cycles, parents, colors);
            }

            return GetUniqueCycles(cycles);
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        /// <param name="graph">The layout graph.</param>
        /// <param name="cycles">A list of cycle chains.</param>
        /// <param name="parents">A dictionary of parent node ID's by node ID.</param>
        /// <param name="colors">A dictionary with the status code of the node by node ID.</param>
        private static void CycleSearch(int node, int parent,
            LayoutGraph graph, List<List<int>> cycles, Dictionary<int, int> parents, Dictionary<int, int> colors)
        {
            colors.TryGetValue(node, out var color);

            // Tree traversal from this node is already complete.
            if (color == 2)
                return;

            // Tree traversal from this node is in progress.
            // Therefore, the tree has looped back around, forming a cycle.
            if (color == 1)
            {
                var current = parent;
                var cycle = new List<int> { current };
                cycles.Add(cycle);

                // Accumulate parents into cycle until origin node is encountered.
                while (current != node)
                {
                    current = parents[current];
                    cycle.Add(current);
                }

                return;
            }

            // Change color to indicate search is in progress for node.
            colors[node] = 1;
            parents[node] = parent;

            foreach (var neighbor in graph.GetNeighbors(node))
            {
                if (neighbor != parents[node])
                    CycleSearch(neighbor, node, graph, cycles, parents, colors);
            }

            // Change color to indicate search from node is complete.
            colors[node] = 2;
        }

        /// <summary>
        /// Returns a new list with all unique cycles in the graph.
        /// </summary>
        /// <param name="cycles">A list of cycle chains.</param>
        private static List<List<int>> GetUniqueCycles(List<List<int>> cycles)
        {
            var sets = new List<HashSet<int>>();
            var result = new List<List<int>>();

            foreach (var cycle in cycles)
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
