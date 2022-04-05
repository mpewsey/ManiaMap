using System;
using System.Drawing;

namespace MPewsey.ManiaMap.Samples
{
    /// <summary>
    /// Contains a collection of LayoutGraph.
    /// </summary>
    public static class GraphLibrary
    {
        /// <summary>
        /// Returns a graph in the shape of a cross.
        /// </summary>
        public static LayoutGraph CrossGraph()
        {
            var graph = new LayoutGraph(1, "CrossLayoutGraph");

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(0, 3);
            graph.AddEdge(0, 4);
            graph.AddEdge(0, 5);

            foreach (var node in graph.GetNodes())
            {
                node.TemplateGroup = "Default";
            }

            return graph;
        }

        /// <summary>
        /// Returns a graph with two loops that share segments.
        /// </summary>
        public static LayoutGraph LoopGraph()
        {
            var graph = new LayoutGraph(2, "LoopLayoutGraph");

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 0);

            graph.AddEdge(0, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            graph.AddEdge(7, 3);

            foreach (var node in graph.GetNodes())
            {
                node.TemplateGroup = "Default";
            }

            return graph;
        }

        /// <summary>
        /// Returns the graph shown at https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/.
        /// </summary>
        public static LayoutGraph GeekGraph()
        {
            var graph = new LayoutGraph(3, "GeekLayoutGraph");

            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 6);
            graph.AddEdge(4, 7);
            graph.AddEdge(5, 6);
            graph.AddEdge(3, 5);
            graph.AddEdge(7, 8);
            graph.AddEdge(6, 10);
            graph.AddEdge(5, 9);
            graph.AddEdge(10, 11);
            graph.AddEdge(11, 12);
            graph.AddEdge(11, 13);
            graph.AddEdge(12, 13);

            foreach (var node in graph.GetNodes())
            {
                node.TemplateGroup = "Default";
            }

            return graph;
        }

        /// <summary>
        /// Returns a LoopGraph with some nodes set to different Z (layer) values.
        /// </summary>
        public static LayoutGraph StackedLoopGraph()
        {
            var graph = new LayoutGraph(4, "StackedLoopLayoutGraph");

            graph.AddNode(0).SetZ(2);
            graph.AddNode(1).SetZ(2);
            graph.AddNode(2).SetZ(2);
            graph.AddNode(3).SetZ(-1);

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 0);

            graph.AddEdge(0, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);
            graph.AddEdge(7, 3);

            foreach (var node in graph.GetNodes())
            {
                node.TemplateGroup = "Default";
            }

            return graph;
        }

        /// <summary>
        /// Returns the graph for the big layout.
        /// </summary>
        public static LayoutGraph BigGraph()
        {
            var graph = new LayoutGraph(5, "BigLayoutGraph");

            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 9);
            graph.AddEdge(4, 6);
            graph.AddEdge(6, 7);
            graph.AddEdge(7, 8);
            graph.AddEdge(9, 10);
            graph.AddEdge(1, 10);
            graph.AddEdge(3, 34);
            graph.AddEdge(9, 30);
            graph.AddEdge(10, 11);
            graph.AddEdge(11, 12);
            graph.AddEdge(12, 13);
            graph.AddEdge(13, 14);
            graph.AddEdge(13, 15);
            graph.AddEdge(13, 16);
            graph.AddEdge(12, 17);
            graph.AddEdge(17, 18);
            graph.AddEdge(18, 19);
            graph.AddEdge(19, 20);
            graph.AddEdge(20, 21);
            graph.AddEdge(21, 24);
            graph.AddEdge(24, 25);
            graph.AddEdge(25, 26);
            graph.AddEdge(21, 22);
            graph.AddEdge(22, 23);
            graph.AddEdge(23, 29);
            graph.AddEdge(30, 31);
            graph.AddEdge(31, 32);
            graph.AddEdge(32, 33);
            graph.AddEdge(33, 34);
            graph.AddEdge(31, 28);
            graph.AddEdge(27, 28);
            graph.AddEdge(28, 29);

            foreach (var node in graph.GetNodes())
            {
                node.TemplateGroup = "Rooms";

                if (node.Id <= 10)
                    node.Color = Color.Blue;
                else if (node.Id <= 18)
                    node.Color = Color.Green;
                else if (node.Id <= 26)
                    node.Color = Color.Purple;
                else if (node.Id <= 34)
                    node.Color = Color.Red;
            }

            foreach (var edge in graph.GetEdges())
            {
                edge.RoomChance = 0.5f;
                edge.TemplateGroup = "Paths";
                var id = Math.Min(edge.FromNode, edge.ToNode);

                if (id <= 10)
                    edge.Color = Color.Blue;
                else if (id <= 18)
                    edge.Color = Color.Green;
                else if (id <= 26)
                    edge.Color = Color.Purple;
                else if (id <= 34)
                    edge.Color = Color.Red;
            }

            return graph;
        }
    }
}
