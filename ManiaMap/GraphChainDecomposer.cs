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
            var chains = new List<List<LayoutEdge>>();
            AddCycleChains(graph, chains);
            AddBranchChains(graph, chains);
            RemoveDuplicateEdges(chains);
            SplitBrokenChains(chains);
            return FormSequentialChains(chains);
        }

        /// <summary>
        /// Returns a list of chains for all cycles in the graph.
        /// </summary>
        private static void AddCycleChains(LayoutGraph graph, List<List<LayoutEdge>> chains)
        {
            var cycles = GraphCycleDecomposer.FindCycles(graph);
            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));

            foreach (var cycle in cycles)
            {
                cycle.Add(cycle[0]);
                chains.Add(GetChainEdges(graph, cycle));
            }
        }

        /// <summary>
        /// Adds chains for all branches of the graph to the input list.
        /// </summary>
        private static void AddBranchChains(LayoutGraph graph, List<List<LayoutEdge>> chains)
        {
            var branches = GraphBranchDecomposer.FindBranches(graph);

            foreach (var branch in branches)
            {
                chains.Add(GetChainEdges(graph, branch));
            }
        }

        /// <summary>
        /// Splits any non sequential chains into separate chains and returns a new list.
        /// </summary>
        private static void SplitBrokenChains(List<List<LayoutEdge>> chains)
        {
            for (int i = 0; i < chains.Count; i++)
            {
                var chain = chains[i];

                for (int j = 1; j < chain.Count; j++)
                {
                    var x = chain[j - 1];
                    var y = chain[j];

                    if (x.FromNode != y.FromNode && x.FromNode != y.ToNode && x.ToNode != y.FromNode && x.ToNode != y.ToNode)
                    {
                        chains[i] = chain.GetRange(0, j);
                        chains.Insert(i + 1, chain.GetRange(j, chain.Count - j));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new list of sequential chains.
        /// </summary>
        private static List<List<LayoutEdge>> FormSequentialChains(List<List<LayoutEdge>> chains)
        {
            var marked = new HashSet<int>();
            var result = new List<List<LayoutEdge>>(chains.Count);
            var pool = new LinkedList<List<LayoutEdge>>(chains);

            if (pool.First != null)
            {
                var chain = pool.First.Value;
                MarkNodes(chain, marked);
                result.Add(chain);
                pool.RemoveFirst();
            }

            for (int i = 1; i < chains.Count; i++)
            {
                var chain = FindNextChain(pool, marked);
                MarkNodes(chain, marked);
                result.Add(chain);
            }

            return result;
        }

        /// <summary>
        /// Returns the next chain in the sequence.
        /// </summary>
        private static List<LayoutEdge> FindNextChain(LinkedList<List<LayoutEdge>> pool, HashSet<int> marked)
        {
            for (var node = pool.First; node != null; node = node.Next)
            {
                var chain = node.Value;

                // Check if first edge forms sequence.
                var first = chain[0];
                
                if (marked.Contains(first.FromNode) || marked.Contains(first.ToNode))
                {
                    pool.Remove(node);
                    return chain;
                }

                // Check if last edge forms sequence.
                var last = chain[^1];

                if (marked.Contains(last.FromNode) || marked.Contains(last.ToNode))
                {
                    pool.Remove(node);
                    chain.Reverse();
                    return chain;
                }

                // If the chain is a cycle, shift the elements of the chain to make a possible sequence.
                if (ChainIsCycle(chain))
                {
                    var index = chain.FindIndex(x => marked.Contains(x.FromNode) || marked.Contains(x.ToNode));

                    if (index >= 0)
                    {
                        pool.Remove(node);
                        chain.AddRange(chain.GetRange(0, index));
                        chain.RemoveRange(0, index);
                        return chain;
                    }
                }
            }

            throw new Exception("Failed to find next chain in sequence.");
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

        /// <summary>
        /// Returns true if the chain is a cycle.
        /// </summary>
        private static bool ChainIsCycle(List<LayoutEdge> chain)
        {
            if (chain.Count > 2)
            {
                var x = chain[0];
                var y = chain[^1];
                return x.FromNode == y.FromNode || x.FromNode == y.ToNode || x.ToNode == y.FromNode || x.ToNode == y.ToNode;
            }

            return false;
        }
    }
}
