using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for finding chains of a `LayoutGraph`.
    /// </summary>
    public static class GraphChainDecomposer
    {
        /// <summary>
        /// Returns a new list of chains for the graph.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="maxBranchLength">The maximum branch chain length. Branch chains exceeding this length will be split. Negative and zero values will be ignored.</param>
        public static List<List<LayoutEdge>> FindChains(LayoutGraph graph, int maxBranchLength = -1)
        {
            var chains = new List<List<LayoutEdge>>();
            AddCycleChains(graph, chains);
            AddBranchChains(graph, chains);
            RemoveDuplicateEdges(chains);
            SplitBrokenChains(chains);
            SplitLongChains(maxBranchLength, chains);
            OrderEdgeNodes(chains);
            return FormSequentialChains(chains);
        }

        /// <summary>
        /// Returns a list of chains for all cycles in the graph.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="chains">A list of chains.</param>
        private static void AddCycleChains(LayoutGraph graph, List<List<LayoutEdge>> chains)
        {
            var cycles = graph.FindCycles();
            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));

            foreach (var cycle in cycles)
            {
                cycle.Add(cycle[0]);
                chains.Add(GetChainEdges(graph, cycle));
            }
        }

        /// <summary>
        /// Reverses the edges in the chains as necessary to make the nodes
        /// in the chain sequential.
        /// </summary>
        /// <param name="chains">A list of chains.</param>
        private static void OrderEdgeNodes(List<List<LayoutEdge>> chains)
        {
            foreach (var chain in chains)
            {
                for (int i = 1; i < chain.Count; i++)
                {
                    var back = chain[i - 1];
                    var forward = chain[i];

                    if (back.FromNode == forward.ToNode)
                    {
                        back.Reverse();
                        forward.Reverse();
                    }
                    else if (back.FromNode == forward.FromNode)
                    {
                        back.Reverse();
                    }
                    else if (back.ToNode == forward.ToNode)
                    {
                        forward.Reverse();
                    }
                }
            }
        }

        /// <summary>
        /// Adds chains for all branches of the graph to the input list.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="chains">A list of chains.</param>
        private static void AddBranchChains(LayoutGraph graph, List<List<LayoutEdge>> chains)
        {
            var branches = graph.FindBranches();

            foreach (var branch in branches)
            {
                chains.Add(GetChainEdges(graph, branch));
            }
        }

        /// <summary>
        /// Removes any duplicate edges from the list of chains. The first
        /// occurence of an edge is retained.
        /// </summary>
        /// <param name="chains">A list of chains.</param>
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
        /// Splits any non sequential chains into separate chains and returns a new list.
        /// </summary>
        /// <param name="chains">A list of chains.</param>
        private static void SplitBrokenChains(List<List<LayoutEdge>> chains)
        {
            for (int i = 0; i < chains.Count; i++)
            {
                var chain = chains[i];

                for (int j = 1; j < chain.Count; j++)
                {
                    var x = chain[j - 1];
                    var y = chain[j];

                    if (!x.SharesNode(y))
                    {
                        chains[i] = chain.GetRange(0, j);
                        chains.Insert(i + 1, chain.GetRange(j, chain.Count - j));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Splits non cycle chains which exceed the specified max length. If the max
        /// length is negative or zero, no action will be taken.
        /// </summary>
        /// <param name="maxBranchLength">The maximum branch length.</param>
        /// <param name="chains">A list of chains.</param>
        private static void SplitLongChains(int maxBranchLength, List<List<LayoutEdge>> chains)
        {
            if (maxBranchLength > 0)
            {
                for (int i = 0; i < chains.Count; i++)
                {
                    var chain = chains[i];

                    if (!ChainIsCycle(chain) && chain.Count > maxBranchLength)
                    {
                        chains[i] = chain.GetRange(0, maxBranchLength);
                        chains.Insert(i + 1, chain.GetRange(maxBranchLength, chain.Count - maxBranchLength));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new list of sequential chains.
        /// </summary>
        /// <param name="chains">A list of chains.</param>
        private static List<List<LayoutEdge>> FormSequentialChains(List<List<LayoutEdge>> chains)
        {
            var pool = new LinkedList<List<LayoutEdge>>();
            var marked = new HashSet<int>();
            var result = new List<List<LayoutEdge>>(chains.Count);

            foreach (var chain in chains)
            {
                pool.AddLast(chain);
            }

            if (pool.First != null)
            {
                var chain = pool.First.Value;
                MarkNodes(marked, chain);
                result.Add(chain);
                pool.RemoveFirst();
            }

            for (int i = 1; i < chains.Count; i++)
            {
                var chain = FindNextChain(pool, marked);
                MarkNodes(marked, chain);
                result.Add(chain);
            }

            return result;
        }

        /// <summary>
        /// Returns the next chain in the sequence.
        /// </summary>
        /// <param name="pool">The chain pool.</param>
        /// <param name="marked">The set of marked nodes.</param>
        private static List<LayoutEdge> FindNextChain(LinkedList<List<LayoutEdge>> pool, HashSet<int> marked)
        {
            for (var node = pool.First; node != null; node = node.Next)
            {
                var chain = node.Value;

                // If the chain is a cycle, shift the elements of the chain to make a possible sequence.
                if (ChainIsCycle(chain))
                {
                    var index = chain.FindIndex(x => marked.Contains(x.FromNode));

                    if (index >= 0)
                    {
                        pool.Remove(node);
                        chain.AddRange(chain.GetRange(0, index));
                        chain.RemoveRange(0, index);
                        return chain;
                    }

                    continue;
                }

                // Check if first edge forms sequence.
                if (marked.Contains(chain[0].FromNode))
                {
                    pool.Remove(node);
                    return chain;
                }

                // Check if last edge forms sequence.
                if (marked.Contains(chain[chain.Count - 1].ToNode))
                {
                    pool.Remove(node);
                    chain.Reverse();
                    chain.ForEach(x => x.Reverse());
                    return chain;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds the nodes in the chain to the marked set.
        /// </summary>
        /// <param name="chain">A list of chain edges.</param>
        private static void MarkNodes(HashSet<int> marked, List<LayoutEdge> chain)
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
        /// <param name="graph">The layout graph.</param>
        /// <param name="nodes">A list of node ID's.</param>
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
        /// Returns true if the chain is a cycle.
        /// </summary>
        /// <param name="chain">A list of chain edges.</param>
        private static bool ChainIsCycle(List<LayoutEdge> chain)
        {
            return chain.Count > 2 && chain[0].SharesNode(chain[chain.Count - 1]);
        }
    }
}
