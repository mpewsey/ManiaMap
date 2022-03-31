using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for finding chains of a `LayoutGraph`.
    /// </summary>
    public static class GraphChainDecomposer
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
            /// The maximum branch chain length. Branch chains exceeding this length will be split.
            /// Negative and zero values will be ignored.
            /// </summary>
            public int MaxBranchLength { get; }

            /// <summary>
            /// A list of chains.
            /// </summary>
            public List<List<LayoutEdge>> Chains { get; } = new List<List<LayoutEdge>>();

            /// <summary>
            /// A set of all marked nodes.
            /// </summary>
            public HashSet<int> Marked { get; } = new HashSet<int>();

            /// <summary>
            /// A pool of chains.
            /// </summary>
            public LinkedList<List<LayoutEdge>> Pool { get; } = new LinkedList<List<LayoutEdge>>();

            /// <summary>
            /// Initializes the data.
            /// </summary>
            /// <param name="graph">The layout graph.</param>
            /// <param name="maxBranchLength">The maximum branch length.</param>
            public Data(LayoutGraph graph, int maxBranchLength)
            {
                Graph = graph;
                MaxBranchLength = maxBranchLength;
            }
        }

        /// <summary>
        /// Returns a new list of chains for the graph.
        /// </summary>
        /// <param name="graph">The layout graph.</param>
        /// <param name="maxBranchLength"> The maximum branch chain length. Branch chains exceeding this length will be split. Negative and zero values will be ignored.</param>
        public static List<List<LayoutEdge>> FindChains(LayoutGraph graph, int maxBranchLength = -1)
        {
            var data = new Data(graph, maxBranchLength);

            AddCycleChains(data);
            AddBranchChains(data);
            RemoveDuplicateEdges(data);
            SplitBrokenChains(data);
            SplitLongChains(data);
            OrderEdgeNodes(data);

            return FormSequentialChains(data);
        }

        /// <summary>
        /// Returns a list of chains for all cycles in the graph.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void AddCycleChains(Data data)
        {
            var cycles = data.Graph.FindCycles();
            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));

            foreach (var cycle in cycles)
            {
                cycle.Add(cycle[0]);
                data.Chains.Add(GetChainEdges(data, cycle));
            }
        }

        /// <summary>
        /// Reverses the edges in the chains as necessary to make the nodes
        /// in the chain sequential.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void OrderEdgeNodes(Data data)
        {
            foreach (var chain in data.Chains)
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
        /// <param name="data">The decomposer data.</param>
        private static void AddBranchChains(Data data)
        {
            var branches = data.Graph.FindBranches();

            foreach (var branch in branches)
            {
                data.Chains.Add(GetChainEdges(data, branch));
            }
        }

        /// <summary>
        /// Removes any duplicate edges from the list of chains. The first
        /// occurence of an edge is retained.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void RemoveDuplicateEdges(Data data)
        {
            var marked = new HashSet<LayoutEdge>();

            foreach (var chain in data.Chains)
            {
                for (int i = chain.Count - 1; i >= 0; i--)
                {
                    if (!marked.Add(chain[i]))
                    {
                        chain.RemoveAt(i);
                    }
                }
            }

            data.Chains.RemoveAll(x => x.Count == 0);
        }

        /// <summary>
        /// Splits any non sequential chains into separate chains and returns a new list.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void SplitBrokenChains(Data data)
        {
            for (int i = 0; i < data.Chains.Count; i++)
            {
                var chain = data.Chains[i];

                for (int j = 1; j < chain.Count; j++)
                {
                    var x = chain[j - 1];
                    var y = chain[j];

                    if (!x.SharesNode(y))
                    {
                        data.Chains[i] = chain.GetRange(0, j);
                        data.Chains.Insert(i + 1, chain.GetRange(j, chain.Count - j));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Splits non cycle chains which exceed the specified max length. If the max
        /// length is negative or zero, no action will be taken.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static void SplitLongChains(Data data)
        {
            if (data.MaxBranchLength > 0)
            {
                for (int i = 0; i < data.Chains.Count; i++)
                {
                    var chain = data.Chains[i];

                    if (!ChainIsCycle(chain) && chain.Count > data.MaxBranchLength)
                    {
                        data.Chains[i] = chain.GetRange(0, data.MaxBranchLength);
                        data.Chains.Insert(i + 1, chain.GetRange(data.MaxBranchLength, chain.Count - data.MaxBranchLength));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new list of sequential chains.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static List<List<LayoutEdge>> FormSequentialChains(Data data)
        {
            var result = new List<List<LayoutEdge>>(data.Chains.Count);

            foreach (var chain in data.Chains)
            {
                data.Pool.AddLast(chain);
            }

            if (data.Pool.First != null)
            {
                var chain = data.Pool.First.Value;
                MarkNodes(data, chain);
                result.Add(chain);
                data.Pool.RemoveFirst();
            }

            for (int i = 1; i < data.Chains.Count; i++)
            {
                var chain = FindNextChain(data);
                MarkNodes(data, chain);
                result.Add(chain);
            }

            return result;
        }

        /// <summary>
        /// Returns the next chain in the sequence.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        private static List<LayoutEdge> FindNextChain(Data data)
        {
            for (var node = data.Pool.First; node != null; node = node.Next)
            {
                var chain = node.Value;

                // If the chain is a cycle, shift the elements of the chain to make a possible sequence.
                if (ChainIsCycle(chain))
                {
                    var index = chain.FindIndex(x => data.Marked.Contains(x.FromNode));

                    if (index >= 0)
                    {
                        data.Pool.Remove(node);
                        var subChain = chain.GetRange(0, index);
                        chain.RemoveRange(0, index);
                        chain.AddRange(subChain);
                        return chain;
                    }

                    continue;
                }

                // Check if first edge forms sequence.
                if (data.Marked.Contains(chain[0].FromNode))
                {
                    data.Pool.Remove(node);
                    return chain;
                }

                // Check if last edge forms sequence.
                if (data.Marked.Contains(chain[chain.Count - 1].ToNode))
                {
                    data.Pool.Remove(node);
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
        /// <param name="data">The decomposer data.</param>
        /// <param name="chain">A list of chain edges.</param>
        private static void MarkNodes(Data data, List<LayoutEdge> chain)
        {
            foreach (var edge in chain)
            {
                data.Marked.Add(edge.FromNode);
                data.Marked.Add(edge.ToNode);
            }
        }

        /// <summary>
        /// Returns a new list of edges based on a list of nodes.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        /// <param name="nodes">A list of node ID's.</param>
        private static List<LayoutEdge> GetChainEdges(Data data, List<int> nodes)
        {
            var chain = new List<LayoutEdge>(nodes.Count);

            for (int i = nodes.Count - 1; i >= 1; i--)
            {
                chain.Add(data.Graph.GetEdge(nodes[i - 1], nodes[i]));
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
