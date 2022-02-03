using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class LayoutGraph
    {
        private Dictionary<int, LayoutNode> Nodes { get; } = new();
        private Dictionary<EdgeIndexes, LayoutEdge> Edges { get; } = new();
        private Dictionary<int, List<int>> Neighbors { get; } = new();

        public override string ToString()
        {
            return $"LayoutGraph(Nodes.Count = {Nodes.Count}, Edges.Count = {Edges.Count})";
        }

        public LayoutNode AddNode(int id)
        {
            if (!Nodes.TryGetValue(id, out var node))
            {
                node = new LayoutNode(id);
                Nodes.Add(id, node);
            }

            if (!Neighbors.ContainsKey(id))
            {
                Neighbors.Add(id, new());
            }

            return node;
        }

        public LayoutEdge AddEdge(int fromNode, int toNode)
        {
            if (!TryGetEdge(fromNode, toNode, out var edge))
            {
                edge = new LayoutEdge(fromNode, toNode);
                Edges.Add(new(fromNode, toNode), edge);
            }

            AddNode(fromNode);
            AddNode(toNode);

            var fromNeighbors = Neighbors[fromNode];
            var toNeighbors = Neighbors[toNode];

            if (!fromNeighbors.Contains(toNode))
                fromNeighbors.Add(toNode);

            if (!toNeighbors.Contains(fromNode))
                toNeighbors.Add(fromNode);

            return edge;
        }

        public void RemoveNode(int id)
        {
            var neighbors = Neighbors[id];

            for (int i = neighbors.Count - 1; i >= 0; i--)
            {
                var neighbor = neighbors[i];
                Neighbors[neighbor].Remove(id);

                if (!Edges.Remove(new(id, neighbor)))
                    Edges.Remove(new(neighbor, id));
            }

            Neighbors.Remove(id);
            Nodes.Remove(id);
        }

        public void RemoveEdge(int node1, int node2)
        {
            Neighbors[node1].Remove(node2);
            Neighbors[node2].Remove(node1);

            if (!Edges.Remove(new(node1, node2)))
                Edges.Remove(new(node2, node1));
        }

        public LayoutNode GetNode(int id)
        {
            return Nodes[id];
        }

        public bool TryGetEdge(int node1, int node2, out LayoutEdge edge)
        {
            if (Edges.TryGetValue(new(node1, node2), out edge))
                return true;
            if (Edges.TryGetValue(new(node2, node1), out edge))
                return true;
            return false;
        }

        public LayoutEdge GetEdge(int node1, int node2)
        {
            if (Edges.TryGetValue(new(node2, node1), out var edge))
                return edge;
            return Edges[new(node1, node2)];
        }

        public IEnumerable<LayoutNode> GetNodes()
        {
            return Nodes.Values;
        }

        public IEnumerable<LayoutEdge> GetEdges()
        {
            return Edges.Values;
        }

        public IEnumerable<int> GetNeighbors(int id)
        {
            return Neighbors[id];
        }

        public List<List<LayoutEdge>> GetChains()
        {
            var cycles = GetCycles().OrderBy(x => x.Count).Select(GetChainFromCycleNodes).ToList();
            var result = GetChainsFromCycles(cycles);
            var branches = GetBranches().OrderBy(x => x.Count).Select(GetChainFromBranchNodes);
            result.AddRange(branches);
            return result;
        }

        public List<List<int>> GetBranches()
        {
            var cycles = GetCycles();
            var marked = new HashSet<int>(Nodes.Count);
            var parents = new Dictionary<int, int>(Nodes.Count);
            var branches = new List<List<int>>();

            foreach (var cycle in cycles)
            {
                foreach (var node in cycle)
                {
                    marked.Add(node);
                }
            }

            foreach (var node in marked.ToList())
            {
                BranchSearch(node, -1, parents, marked, branches);
            }

            return branches;
        }

        private void BranchSearch(int node, int parent, Dictionary<int, int> parents, HashSet<int> marked, List<List<int>> branches)
        {
            if (parents.ContainsKey(node))
            {
                var branch = new List<int> { parent, node };
                branches.Add(branch);
                marked.Add(node);
                var current = node;
                var isMarked = false;

                while (!isMarked)
                {
                    current = parents[current];
                    branch.Add(current);
                    isMarked = !marked.Add(current);
                }

                return;
            }
            
            var neighbors = Neighbors[node];
            parents[node] = parent;

            if (neighbors.Count == 1)
            {
                var branch = new List<int> { node };
                branches.Add(branch);
                marked.Add(node);
                var current = node;
                var isMarked = false;

                while (!isMarked)
                {
                    current = parents[current];
                    branch.Add(current);
                    isMarked = !marked.Add(current);
                }

                return;
            }

            foreach (var neighbor in neighbors)
            {
                if (!marked.Contains(neighbor))
                    BranchSearch(neighbor, node, parents, marked, branches);
            }
        }

        public List<List<int>> GetCycles()
        {
            var parents = new Dictionary<int, int>(Nodes.Count);
            var colors = new Dictionary<int, int>(Nodes.Count);
            var cycles = new List<List<int>>();

            foreach (var node in Nodes.Keys)
            {
                CycleSearch(node, -1, parents, colors, cycles);
                parents.Clear();
                colors.Clear();
            }

            return GetUniqueCollections(cycles);
        }

        private void CycleSearch(int node, int parent, Dictionary<int, int> parents, Dictionary<int, int> colors, List<List<int>> cycles)
        {
            colors.TryGetValue(node, out var color);

            if (color == 2)
                return;

            if (color == 1)
            {
                var current = parent;
                var cycle = new List<int> { current };
                cycles.Add(cycle);

                while (current != node)
                {
                    current = parents[current];
                    cycle.Add(current);
                }

                return;
            }

            parents[node] = parent;
            colors[node] = 1;

            foreach (var neighbor in Neighbors[node])
            {
                if (neighbor != parents[node])
                    CycleSearch(neighbor, node, parents, colors, cycles);
            }

            colors[node] = 2;
        }

        private static List<List<T>> GetUniqueCollections<T>(List<List<T>> lists)
        {
            var sets = new List<HashSet<T>>();
            var result = new List<List<T>>();

            foreach (var list in lists)
            {
                if (!sets.Any(x => x.SetEquals(list)))
                {
                    result.Add(list);
                    sets.Add(new(list));
                }
            }

            return result;
        }

        private List<LayoutEdge> GetChainFromCycleNodes(List<int> nodes)
        {
            var chain = new List<LayoutEdge>(nodes.Count)
            {
                GetEdge(nodes[0], nodes[^1])
            };

            for (int i = 1; i < nodes.Count; i++)
            {
                chain.Add(GetEdge(nodes[i - 1], nodes[i]));
            }

            return chain;
        }

        private List<LayoutEdge> GetChainFromBranchNodes(List<int> nodes)
        {
            var chain = new List<LayoutEdge>(nodes.Count);

            for (int i = 1; i < nodes.Count; i++)
            {
                chain.Add(GetEdge(nodes[i - 1], nodes[i]));
            }

            return chain;
        }

        private static List<List<LayoutEdge>> GetChainsFromCycles(List<List<LayoutEdge>> cycles)
        {
            var result = new List<List<LayoutEdge>>();

            for (int i = 0; i < cycles.Count; i++)
            {
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    foreach (var edge in cycles[i])
                    {
                        cycles[j].Remove(edge);
                    }
                }

                if (cycles[i].Count > 0)
                    result.Add(cycles[i]);
            }

            return result;
        }
    }
}
