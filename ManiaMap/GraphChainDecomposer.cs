using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public class GraphChainDecomposer
    {
        private LayoutGraph Graph { get; }
        private int MaxBranchLength { get; }
        private HashSet<int> Marked { get; } = new HashSet<int>();
        private LinkedList<List<LayoutEdge>> Pool { get; } = new LinkedList<List<LayoutEdge>>();
        private List<List<LayoutEdge>> Chains { get; } = new List<List<LayoutEdge>>();

        public GraphChainDecomposer(LayoutGraph graph, int maxBranchLength = -1)
        {
            Graph = graph;
            MaxBranchLength = maxBranchLength;
        }

        public override string ToString()
        {
            return $"GraphChainDecomposer(Graph = {Graph}, MaxBranchLength = {MaxBranchLength})";
        }

        /// <summary>
        /// Returns a new list of chains for the graph.
        /// </summary>
        public List<List<LayoutEdge>> FindChains()
        {
            Chains.Clear();
            AddCycleChains();
            AddBranchChains();
            RemoveDuplicateEdges();
            SplitBrokenChains();
            SplitLongChains();
            OrderEdgeNodes();
            return FormSequentialChains();
        }

        /// <summary>
        /// Returns a list of chains for all cycles in the graph.
        /// </summary>
        private void AddCycleChains()
        {
            var cycles = Graph.FindCycles();
            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));

            foreach (var cycle in cycles)
            {
                cycle.Add(cycle[0]);
                Chains.Add(GetChainEdges(cycle));
            }
        }

        /// <summary>
        /// Reverses the edges in the chains as necessary to make the nodes
        /// in the chain sequential.
        /// </summary>
        private void OrderEdgeNodes()
        {
            foreach (var chain in Chains)
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
        private void AddBranchChains()
        {
            var branches = Graph.FindBranches();

            foreach (var branch in branches)
            {
                Chains.Add(GetChainEdges(branch));
            }
        }

        /// <summary>
        /// Removes any duplicate edges from the list of chains. The first
        /// occurence of an edge is retained.
        /// </summary>
        private void RemoveDuplicateEdges()
        {
            var marked = new HashSet<LayoutEdge>();

            foreach (var chain in Chains)
            {
                for (int i = chain.Count - 1; i >= 0; i--)
                {
                    if (!marked.Add(chain[i]))
                    {
                        chain.RemoveAt(i);
                    }
                }
            }

            Chains.RemoveAll(x => x.Count == 0);
        }

        /// <summary>
        /// Splits any non sequential chains into separate chains and returns a new list.
        /// </summary>
        private void SplitBrokenChains()
        {
            for (int i = 0; i < Chains.Count; i++)
            {
                var chain = Chains[i];

                for (int j = 1; j < chain.Count; j++)
                {
                    var x = chain[j - 1];
                    var y = chain[j];

                    if (x.FromNode != y.FromNode && x.FromNode != y.ToNode
                        && x.ToNode != y.FromNode && x.ToNode != y.ToNode)
                    {
                        Chains[i] = chain.GetRange(0, j);
                        Chains.Insert(i + 1, chain.GetRange(j, chain.Count - j));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Splits non cycle chains which exceed the specified max length. If the max
        /// length is negative or zero, no action will be taken.
        /// </summary>
        private void SplitLongChains()
        {
            if (MaxBranchLength > 0)
            {
                for (int i = 0; i < Chains.Count; i++)
                {
                    var chain = Chains[i];

                    if (!ChainIsCycle(chain) && chain.Count > MaxBranchLength)
                    {
                        Chains[i] = chain.GetRange(0, MaxBranchLength);
                        Chains.Insert(i + 1, chain.GetRange(MaxBranchLength, chain.Count - MaxBranchLength));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new list of sequential chains.
        /// </summary>
        private List<List<LayoutEdge>> FormSequentialChains()
        {
            Pool.Clear();
            Marked.Clear();
            var result = new List<List<LayoutEdge>>(Chains.Count);

            foreach (var chain in Chains)
            {
                Pool.AddLast(chain);
            }

            if (Pool.First != null)
            {
                var chain = Pool.First.Value;
                MarkNodes(chain);
                result.Add(chain);
                Pool.RemoveFirst();
            }

            for (int i = 1; i < Chains.Count; i++)
            {
                var chain = FindNextChain();
                MarkNodes(chain);
                result.Add(chain);
            }

            return result;
        }

        /// <summary>
        /// Returns the next chain in the sequence.
        /// </summary>
        private List<LayoutEdge> FindNextChain()
        {
            for (var node = Pool.First; node != null; node = node.Next)
            {
                var chain = node.Value;

                // If the chain is a cycle, shift the elements of the chain to make a possible sequence.
                if (ChainIsCycle(chain))
                {
                    var index = chain.FindIndex(AnyNodeMarked);

                    if (index >= 0)
                    {
                        Pool.Remove(node);
                        chain.AddRange(chain.GetRange(0, index));
                        chain.RemoveRange(0, index);
                        return chain;
                    }

                    continue;
                }

                // Check if first edge forms sequence.
                if (AnyNodeMarked(chain[0]))
                {
                    Pool.Remove(node);
                    return chain;
                }

                // Check if last edge forms sequence.
                if (AnyNodeMarked(chain[chain.Count - 1]))
                {
                    Pool.Remove(node);
                    chain.Reverse();
                    chain.ForEach(x => x.Reverse());
                    return chain;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns true if any node in the edge is marked.
        /// </summary>
        private bool AnyNodeMarked(LayoutEdge edge)
        {
            return Marked.Contains(edge.FromNode)
                || Marked.Contains(edge.ToNode);
        }

        /// <summary>
        /// Adds the nodes in the chain to the marked set.
        /// </summary>
        private void MarkNodes(List<LayoutEdge> chain)
        {
            foreach (var edge in chain)
            {
                Marked.Add(edge.FromNode);
                Marked.Add(edge.ToNode);
            }
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

        /// <summary>
        /// Returns true if the chain is a cycle.
        /// </summary>
        private static bool ChainIsCycle(List<LayoutEdge> chain)
        {
            if (chain.Count > 2)
            {
                var x = chain[0];
                var y = chain[chain.Count - 1];

                return x.FromNode == y.FromNode || x.FromNode == y.ToNode
                    || x.ToNode == y.FromNode || x.ToNode == y.ToNode;
            }

            return false;
        }
    }
}
