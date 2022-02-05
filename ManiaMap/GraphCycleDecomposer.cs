using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public static class GraphCycleDecomposer
    {
        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        public static List<List<int>> FindCycles(LayoutGraph graph)
        {
            var cycles = new List<List<int>>();
            var parents = new Dictionary<int, int>(graph.NodeCount());
            var colors = new Dictionary<int, int>(graph.NodeCount());

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in graph.GetNodeIds())
            {
                CycleSearch(graph, node, -1, parents, colors, cycles);
                parents.Clear();
                colors.Clear();
            }

            return GetUniqueCycles(cycles);
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
        private static void CycleSearch(LayoutGraph graph, int node, int parent, Dictionary<int, int> parents, Dictionary<int, int> colors, List<List<int>> cycles)
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
                    CycleSearch(graph, neighbor, node, parents, colors, cycles);
            }

            // Change color to indicate search from node is complete.
            colors[node] = 2;
        }

        /// <summary>
        /// Returns a new list with all unique cycles in the graph.
        /// </summary>
        private static List<List<int>> GetUniqueCycles(List<List<int>> cycles)
        {
            var sets = new List<HashSet<int>>();
            var result = new List<List<int>>();

            foreach (var cycle in cycles)
            {
                if (!sets.Any(x => x.SetEquals(cycle)))
                {
                    result.Add(cycle);
                    sets.Add(new(cycle));
                }
            }

            return result;
        }
    }
}
