using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class GraphChainDecomposer
    {
        private LayoutGraph Graph { get; }
        private GraphCycleDecomposer CycleDecomposer { get; }
        private GraphBranchDecomposer BranchDecomposer { get; }
        private List<List<LayoutEdge>> Chains { get; } = new();

        public GraphChainDecomposer(LayoutGraph graph)
        {
            Graph = graph;
            CycleDecomposer = new GraphCycleDecomposer(graph);
            BranchDecomposer = new GraphBranchDecomposer(graph);
        }

        public List<List<LayoutEdge>> FindChains()
        {
            Chains.Clear();
            AddCycleChains();
            AddBranchChains();
            return new(Chains);
        }

        private void AddCycleChains()
        {
            var cycles = CycleDecomposer.FindCycles();

            foreach (var cycle in cycles)
            {
                var chain = new List<LayoutEdge>(cycle.Count);
                Chains.Add(chain);
                chain.Add(Graph.GetEdge(cycle[0], cycle[^1]));

                for (int i = 1; i < cycle.Count; i++)
                {
                    chain.Add(Graph.GetEdge(cycle[i - 1], cycle[i]));
                }
            }
        }

        private void AddBranchChains()
        {
            var branches = BranchDecomposer.FindBranches();
            
            foreach (var branch in branches)
            {
                var chain = new List<LayoutEdge>(branch.Count);
                Chains.Add(chain);

                for (int i = branch.Count - 1; i >= 1; i--)
                {
                    chain.Add(Graph.GetEdge(branch[i - 1], branch[i]));
                }
            }
        }
    }
}
