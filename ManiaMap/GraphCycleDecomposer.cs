using System;
using System.Collections.Generic;
using System.Linq;

namespace ManiaMap
{
    public class GraphCycleDecomposer
    {
        private LayoutGraph Graph { get; }
        private Dictionary<int, int> Parents { get; }
        private Dictionary<int, int> Colors { get; }
        private List<List<int>> Cycles { get; } = new List<List<int>>();

        public GraphCycleDecomposer(LayoutGraph graph)
        {
            Graph = graph;
            Parents = new Dictionary<int, int>(Graph.NodeCount());
            Colors = new Dictionary<int, int>(Graph.NodeCount());
        }

        public override string ToString()
        {
            return $"GraphCycleDecomposer(Graph = {Graph})";
        }

        /// <summary>
        /// Returns lists of all combinations of unique node cycles in the graph
        /// using depth first search.
        /// </summary>
        public List<List<int>> FindCycles()
        {
            Cycles.Clear();

            // Run searches from every node to accumulate complete set of cycles.
            foreach (var node in Graph.GetNodeIds())
            {
                Parents.Clear();
                Colors.Clear();
                CycleSearch(node, -1);
            }

            return GetUniqueCycles();
        }

        /// <summary>
        /// Performs depth first search to find cycles in the graph.
        /// </summary>
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
