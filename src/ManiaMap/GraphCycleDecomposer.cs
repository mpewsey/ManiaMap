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
    public static class GraphCycleDecomposer
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
            /// A list of cycles in the graph.
            /// </summary>
            public List<List<int>> Cycles { get; } = new List<List<int>>();

            /// <summary>
            /// A dictionary of node parents by node ID.
            /// </summary>
            public Dictionary<int, int> Parents { get; }

            /// <summary>
            /// A dictionary of node colors by node ID.
            /// </summary>
            public Dictionary<int, int> Colors { get; }

            public Data(LayoutGraph graph)
            {
                Graph = graph;
                Parents = new Dictionary<int, int>(graph.NodeCount);
                Colors = new Dictionary<int, int>(graph.NodeCount);
            }
        }

        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        public static List<List<int>> FindCycles(LayoutGraph graph)
        {
            var data = new Data(graph);

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in data.Graph.GetNodes())
            {
                data.Parents.Clear();
                data.Colors.Clear();
                CycleSearch(data, node.Id, -1);
            }

            return GetUniqueCycles(data);
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        /// <param name="node">The node ID.</param>
        /// <param name="parent">The node's parent ID.</param>
        private static void CycleSearch(Data data, int node, int parent)
        {
            data.Colors.TryGetValue(node, out var color);

            // Tree traversal from this node is already complete.
            if (color == 2)
                return;

            // Tree traversal from this node is in progress.
            // Therefore, the tree has looped back around, forming a cycle.
            if (color == 1)
            {
                var current = parent;
                var cycle = new List<int> { current };
                data.Cycles.Add(cycle);

                // Accumulate parents into cycle until origin node is encountered.
                while (current != node)
                {
                    current = data.Parents[current];
                    cycle.Add(current);
                }

                return;
            }

            // Change color to indicate search is in progress for node.
            data.Colors[node] = 1;
            data.Parents[node] = parent;

            foreach (var neighbor in data.Graph.GetNeighbors(node))
            {
                if (neighbor != data.Parents[node])
                    CycleSearch(data, neighbor, node);
            }

            // Change color to indicate search from node is complete.
            data.Colors[node] = 2;
        }

        /// <summary>
        /// Returns a new list with all unique cycles in the graph.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static List<List<int>> GetUniqueCycles(Data data)
        {
            var sets = new List<HashSet<int>>();
            var result = new List<List<int>>();

            foreach (var cycle in data.Cycles)
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
