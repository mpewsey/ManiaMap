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

        public GraphChainDecomposer(LayoutGraph graph)
        {
            Graph = graph;
        }

        /// <summary>
        /// Returns a new list of chains for the graph.
        /// </summary>
        public List<List<LayoutEdge>> FindChains()
        {
            var chains = new GraphCycleDecomposer(Graph).FindCycles();

            foreach (var chain in chains)
            {
                chain.Add(chain[0]);
            }

            chains.AddRange(new GraphBranchDecomposer(Graph).FindBranches());
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a new list of edges based on a list of nodes.
        /// </summary>
        private List<LayoutEdge> GetChainEdges(List<int> nodes)
        {
            var chain = new List<LayoutEdge>(nodes.Count);

            for (int i = nodes.Count - 1; i >= 1; i--)
            {
                chain.Add(Graph.GetEdge(nodes[i - 1], nodes[i]));
            }

            return chain;
        }
    }
}
