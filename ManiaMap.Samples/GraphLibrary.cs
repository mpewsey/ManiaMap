namespace MPewsey.ManiaMap.Samples
{
    public static class GraphLibrary
    {
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
                node.AddTemplateGroups("Default");
            }

            return graph;
        }

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
                node.AddTemplateGroups("Default");
            }

            return graph;
        }

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
                node.AddTemplateGroups("Default");
            }

            return graph;
        }

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
                node.AddTemplateGroups("Default");
            }

            return graph;
        }
    }
}
