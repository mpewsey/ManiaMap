using System.Drawing;

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
                node.TemplateGroups.Add("Default");
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
                node.TemplateGroups.Add("Default");
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
                node.TemplateGroups.Add("Default");
            }

            return graph;
        }

        public static LayoutGraph FourGiantsGraph()
        {
            var graph = new LayoutGraph(4, "FourGiantsLayoutGraph");

            graph.AddNode(1).SetName("City_SClockTown").AddTemplateGroups("CityLargeRoom");
            graph.AddNode(2).SetName("City_NClockTown").AddTemplateGroups("CityMediumRoom");
            graph.AddNode(3).SetName("City_WClockTown").AddTemplateGroups("CityMediumRoom");
            graph.AddNode(4).SetName("City_EClockTown").AddTemplateGroups("CityMediumRoom");
            graph.AddNode(5).SetName("City_LaundryPool").AddTemplateGroups("CitySmallRoom");
            graph.AddNode(6).SetName("City_ClockTower").AddTemplateGroups("CityBossRoom");

            graph.AddNode(7).SetName("Field_WField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(8).SetName("Field_EField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(9).SetName("Field_SField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(10).SetName("Field_NField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(11).SetName("Field_NWField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(12).SetName("Field_NEField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(13).SetName("Field_SWField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(14).SetName("Field_SEField").AddTemplateGroups("FieldMediumRoom");

            foreach (var node in graph.GetNodes())
            {
                if (node.Name.StartsWith("City"))
                    node.Color = Color.DarkGray;
                else if (node.Name.StartsWith("Field"))
                    node.Color = Color.DarkOliveGreen;
                else if (node.Name.StartsWith("Swamp"))
                    node.Color = Color.ForestGreen;
                else if (node.Name.StartsWith("Mountain"))
                    node.Color = Color.LightBlue;
                else if (node.Name.StartsWith("Bay"))
                    node.Color = Color.SandyBrown;
                else if (node.Name.StartsWith("Canyon"))
                    node.Color = Color.MediumPurple;
            }

            return graph;
        }
    }
}
