using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class GraphBranchDecomposer
    {
        private LayoutGraph Graph { get; }
        private HashSet<int> Marked { get; } = new();
        private Dictionary<int, int> Parents { get; } = new();
        private List<List<int>> Branches { get; } = new();
        private GraphCycleDecomposer CycleDecomposer { get; }

        public GraphBranchDecomposer(LayoutGraph graph)
        {
            Graph = graph;
            CycleDecomposer = new(graph);
        }

        public List<List<int>> FindBranches()
        {
            Marked.Clear();
            Parents.Clear();
            Branches.Clear();
            var cycles = CycleDecomposer.FindCycles();
            
            // Add cycle nodes to marked set.
            foreach (var cycle in cycles)
                foreach (var node in cycle)
                    Marked.Add(node);

            // Search for branches beginning at each cycle node.
            var cycleNodes = Marked.ToArray();

            foreach (var node in cycleNodes)
            {
                BranchSearch(node, -1);
            }

            return new(Branches);
        }

        private void BranchSearch(int node, int parent)
        {
            // If the node already has a parent, then it has been traversed before from another cycle
            // and is a member of a connecting branch between two cycles.
            if (Parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                AddParentsToBranch(node, branch);
                return;
            }

            var neighbors = Graph.GetNeighbors(node);
            Parents[node] = parent;

            // If the node is a deadend node, accumulate the parents into a branch.
            if (neighbors.Count() == 1)
            {
                var branch = new List<int> { node };
                AddParentsToBranch(node, branch);
                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!Marked.Contains(neighbor))
                    BranchSearch(neighbor, node);
            }
        }

        private void AddParentsToBranch(int node, List<int> branch)
        {
            var current = node;
            var isMarked = false;
            Branches.Add(branch);
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
