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

        public List<List<int>> GetCycleNodes()
        {
            int cycle = 0;
            var parents = new Dictionary<int, int>(Nodes.Count);
            var colors = new Dictionary<int, int>(Nodes.Count);
            var cycles = new Dictionary<int, int>(Nodes.Count);
            CycleSearch(Nodes.Keys.Min(), -1, parents, colors, cycles, ref cycle);
            var result = new List<List<int>>();

            foreach (var (node, chain) in cycles)
            {   
                while (result.Count <= chain)
                {
                    result.Add(new());
                }

                result[chain].Add(node);
            }

            return result;
        }

        private void CycleSearch(int node, int parent, Dictionary<int, int> parents, Dictionary<int, int> colors, Dictionary<int, int> cycles, ref int cycle)
        {
            colors.TryGetValue(node, out var color);
            
            if (color == 2)
                return;

            if (color == 1)
            {
                var current = parent;
                cycles[current] = cycle;

                while (current != node)
                {
                    current = parents[current];
                    cycles[current] = cycle;
                }

                cycle++;
                return;
            }

            parents[node] = parent;
            colors[node] = 1;

            foreach (var neighbor in Neighbors[node])
            {
                if (neighbor != parents[node])
                    CycleSearch(neighbor, node, parents, colors, cycles, ref cycle);
            }

            colors[node] = 2;
        }
    }
}
