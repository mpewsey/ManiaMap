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
            var chains = new List<List<LayoutEdge>>();
            var trunk = FindTrunk();

            if (trunk.Count > 1)
            {
                chains.Add(GetCycleChain(trunk));
            }

            var branches = new GraphBranchDecomposer(Graph).FindBranches(trunk);

            foreach (var branch in branches)
            {
                chains.Add(GetBranchChain(branch));
            }

            return chains;
        }

        /// <summary>
        /// Returns the trunk of the graph. If the graph contains cycles, the trunk
        /// is taken as the smallest cycle. Otherwise, the trunk is taken as the
        /// node with the maximum number of neighbors.
        /// </summary>
        private List<int> FindTrunk()
        {
            var cycles = new GraphCycleDecomposer(Graph).FindCycles();

            if (cycles.Count == 0)
            {
                return new() { MaxNeighborNode() };
            }

            return cycles[SmallestListIndex(cycles)];
        }

        /// <summary>
        /// Returns the index of the smallest list in the collection.
        /// </summary>
        private static int SmallestListIndex(List<List<int>> lists)
        {
            var index = -1;
            var minCount = int.MaxValue;

            for (int i = 0; i < lists.Count; i++)
            {
                var list = lists[i];

                if (list.Count < minCount)
                {
                    index = i;
                    minCount = list.Count;
                }
            }

            return index;
        }

        /// <summary>
        /// Returns the node in the graph with the maximum number of neighbors.
        /// </summary>
        private int MaxNeighborNode()
        {
            var maxNode = -1;
            var maxNeighbors = -1;

            foreach (var node in Graph.GetNodeIds())
            {
                var count = Graph.GetNeighbors(node).Count();

                if (count > maxNeighbors)
                {
                    maxNode = node;
                    maxNeighbors = count;
                }
            }

            return maxNode;
        }

        /// <summary>
        /// Returns a new list of edges based on a list of cycles nodes.
        /// </summary>
        private List<LayoutEdge> GetCycleChain(List<int> cycle)
        {
            var chain = new List<LayoutEdge>(cycle.Count)
            {
                Graph.GetEdge(cycle[0], cycle[^1])
            };

            for (int i = 1; i < cycle.Count; i++)
            {
                chain.Add(Graph.GetEdge(cycle[i - 1], cycle[i]));
            }

            return chain;
        }

        /// <summary>
        /// Returns a new list of edges based on a list of branch nodes.
        /// </summary>
        private List<LayoutEdge> GetBranchChain(List<int> branch)
        {
            var chain = new List<LayoutEdge>(branch.Count);

            for (int i = branch.Count - 1; i >= 1; i--)
            {
                chain.Add(Graph.GetEdge(branch[i - 1], branch[i]));
            }

            return chain;
        }
    }
}
