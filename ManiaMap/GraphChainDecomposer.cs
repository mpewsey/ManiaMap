using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public static class GraphChainDecomposer
    {
        /// <summary>
        /// Returns a new list of chains for the graph.
        /// </summary>
        public static List<List<LayoutEdge>> FindChains(LayoutGraph graph)
        {
            var chains = GetCycleChains(graph);
            RemoveDuplicateEdges(chains);
            chains = SplitBrokenChains(chains);
            AddBranchChains(graph, chains);
            return FormSequentialChains(chains);
        }

        private static List<List<LayoutEdge>> GetCycleChains(LayoutGraph graph)
        {
            var cycles = GraphCycleDecomposer.FindCycles(graph);
            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));
            var chains = new List<List<LayoutEdge>>(cycles.Count);

            foreach (var cycle in cycles)
            {
                cycle.Add(cycle[0]);
                chains.Add(GetChainEdges(graph, cycle));
            }

            return chains;
        }

        private static void AddBranchChains(LayoutGraph graph, List<List<LayoutEdge>> chains)
        {
            var branches = GraphBranchDecomposer.FindBranches(graph);

            foreach (var branch in branches)
            {
                chains.Add(GetChainEdges(graph, branch));
            }
        }

        private static List<List<LayoutEdge>> SplitBrokenChains(List<List<LayoutEdge>> chains)
        {
            var result = new List<List<LayoutEdge>>(chains.Count);

            foreach (var chain in chains)
            {
                if (ChainIsSequential(chain))
                {
                    result.Add(chain);
                    continue;
                }

                throw new NotImplementedException();
            }

            return result;
        }

        /// <summary>
        /// Returns a list of sequential chains.
        /// </summary>
        private static List<List<LayoutEdge>> FormSequentialChains(List<List<LayoutEdge>> chains)
        {
            var marked = new HashSet<int>();
            var result = new List<List<LayoutEdge>>(chains.Count);
            var pool = new LinkedList<List<LayoutEdge>>(chains);

            while (pool.First != null)
            {
                if (marked.Count == 0)
                {
                    var chain = pool.First.Value;
                    pool.RemoveFirst();
                    result.Add(chain);
                    MarkNodes(chain, marked);
                    continue;
                }

                for (var node = pool.First; node != null; node = node.Next)
                {
                    var chain = node.Value;
                    var first = chain[0];
                    var last = chain[^1];

                    if (marked.Contains(first.FromNode) || marked.Contains(first.ToNode))
                    {
                        pool.Remove(node);
                        result.Add(chain);
                        MarkNodes(chain, marked);
                        break;
                    }

                    if (marked.Contains(last.FromNode) || marked.Contains(last.ToNode))
                    {
                        pool.Remove(node);
                        chain.Reverse();
                        result.Add(chain);
                        MarkNodes(chain, marked);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the nodes in the chain to the marked set.
        /// </summary>
        private static void MarkNodes(List<LayoutEdge> chain, HashSet<int> marked)
        {
            foreach (var edge in chain)
            {
                marked.Add(edge.FromNode);
                marked.Add(edge.ToNode);
            }
        }

        /// <summary>
        /// Returns a new list of edges based on a list of nodes.
        /// </summary>
        private static List<LayoutEdge> GetChainEdges(LayoutGraph graph, List<int> nodes)
        {
            var chain = new List<LayoutEdge>(nodes.Count);

            for (int i = nodes.Count - 1; i >= 1; i--)
            {
                chain.Add(graph.GetEdge(nodes[i - 1], nodes[i]));
            }

            return chain;
        }

        /// <summary>
        /// Returns true if the chain is sequential.
        /// </summary>
        private static bool ChainIsSequential(List<LayoutEdge> chain)
        {
            for (int i = 1; i < chain.Count; i++)
            {
                var x = chain[i - 1];
                var y = chain[i];

                if (x.FromNode != y.FromNode && x.FromNode != y.ToNode
                    && x.ToNode != y.FromNode && x.ToNode != y.ToNode)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Removes any duplicate edges from the list of chains. The first
        /// occurence of an edge is retained.
        /// </summary>
        private static void RemoveDuplicateEdges(List<List<LayoutEdge>> chains)
        {
            var marked = new HashSet<LayoutEdge>();

            foreach (var chain in chains)
            {
                for (int i = chain.Count - 1; i >= 0; i--)
                {
                    if (!marked.Add(chain[i]))
                    {
                        chain.RemoveAt(i);
                    }
                }
            }

            chains.RemoveAll(x => x.Count == 0);
        }
    }
}
