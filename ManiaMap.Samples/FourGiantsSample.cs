using System.Drawing;

namespace MPewsey.ManiaMap.Samples
{
    public static class FourGiantsSample
    {
        public static TemplateGroups FourGiantsTemplateGroups()
        {
            var templateGroups = new TemplateGroups();

            var smallSquareNotches = TemplateLibrary.SquareNotches.Small().UniqueVariations();
            var mediumSquareNotches = TemplateLibrary.SquareNotches.Medium().UniqueVariations();
            var largeSquareNotches = TemplateLibrary.SquareNotches.Large().UniqueVariations();

            var smallDiamonds = TemplateLibrary.Diamonds.Small().UniqueVariations();
            var mediumDiamonds = TemplateLibrary.Diamonds.Medium().UniqueVariations();
            var largeDiamonds = TemplateLibrary.Diamonds.Large().UniqueVariations();

            var smallPyramids = TemplateLibrary.Pyramids.Small().UniqueVariations();
            var mediumPyramids = TemplateLibrary.Pyramids.Medium().UniqueVariations();
            var largePyramids = TemplateLibrary.Pyramids.Large().UniqueVariations();

            var smallTriangles = TemplateLibrary.RightTriangles.Small().UniqueVariations();
            var mediumTriangles = TemplateLibrary.RightTriangles.Medium().UniqueVariations();
            var largeTriangles = TemplateLibrary.RightTriangles.Large().UniqueVariations();

            var smallAngles = TemplateLibrary.Angles.Small().UniqueVariations();
            var mediumAngles = TemplateLibrary.Angles.Medium().UniqueVariations();
            var largeAngles = TemplateLibrary.Angles.Large().UniqueVariations();

            // Town Groups
            templateGroups.Add("TownSmallRoom", smallSquareNotches, smallDiamonds, smallPyramids, smallTriangles);
            templateGroups.Add("TownMediumRoom", mediumSquareNotches, mediumDiamonds, mediumPyramids, mediumTriangles);
            templateGroups.Add("TownLargeRoom", largeSquareNotches, largeDiamonds, largePyramids, largeTriangles);
            templateGroups.Add("TownBossRoom", largeDiamonds);
            templateGroups.Add("TownPath", smallAngles, mediumAngles, largeAngles);

            // Field Groups
            templateGroups.Add("FieldLargeRoom", largeSquareNotches, largeDiamonds, largePyramids, largeTriangles);
            templateGroups.Add("FieldMediumRoom", mediumSquareNotches, mediumDiamonds, mediumPyramids, mediumTriangles);

            return templateGroups;
        }
        
        public static LayoutGraph FourGiantsGraph()
        {
            var graph = new LayoutGraph(4, "FourGiantsLayoutGraph");

            // Town Nodes
            graph.AddNode(1).SetName("Town_SClockTown").AddTemplateGroups("TownLargeRoom");
            graph.AddNode(2).SetName("Town_NClockTown").AddTemplateGroups("TownMediumRoom");
            graph.AddNode(3).SetName("Town_WClockTown").AddTemplateGroups("TownMediumRoom");
            graph.AddNode(4).SetName("Town_EClockTown").AddTemplateGroups("TownMediumRoom");
            graph.AddNode(5).SetName("Town_LaundryPool").AddTemplateGroups("TownSmallRoom");
            graph.AddNode(6).SetName("Town_ClockTower").AddTemplateGroups("TownBossRoom");
            graph.AddNode(15).SetName("Town_Sewer").AddTemplateGroups("TownPath");
            graph.AddNode(16).SetName("Town_Observatory").AddTemplateGroups("TownSmallRoom");

            // Field Nodes
            graph.AddNode(7).SetName("Field_WField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(8).SetName("Field_EField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(9).SetName("Field_SField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(10).SetName("Field_NField").AddTemplateGroups("FieldLargeRoom");
            graph.AddNode(11).SetName("Field_NWField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(12).SetName("Field_NEField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(13).SetName("Field_SWField").AddTemplateGroups("FieldMediumRoom");
            graph.AddNode(14).SetName("Field_SEField").AddTemplateGroups("FieldMediumRoom");

            // Town Edges
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 1);
            graph.AddEdge(1, 5);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 4);
            graph.AddEdge(1, 6);
            graph.AddEdge(4, 15);
            graph.AddEdge(15, 16);

            // Field Edges
            graph.AddEdge(14, 8);
            graph.AddEdge(14, 9);
            graph.AddEdge(9, 13);
            graph.AddEdge(13, 7);
            graph.AddEdge(7, 10);
            graph.AddEdge(10, 12);
            graph.AddEdge(12, 8);

            // Field to Town Edges
            graph.AddEdge(9, 1);
            graph.AddEdge(3, 7);
            graph.AddEdge(2, 10);
            graph.AddEdge(4, 8);
            graph.AddEdge(16, 8);

            // Set node colors
            foreach (var node in graph.GetNodes())
            {
                if (node.Name.StartsWith("Town"))
                    node.Color = Color.Gray;
                else if (node.Name.StartsWith("Field"))
                    node.Color = Color.ForestGreen;
                else if (node.Name.StartsWith("Swamp"))
                    node.Color = Color.DarkOliveGreen;
                else if (node.Name.StartsWith("Mountain"))
                    node.Color = Color.LightBlue;
                else if (node.Name.StartsWith("Bay"))
                    node.Color = Color.SandyBrown;
                else if (node.Name.StartsWith("Canyon"))
                    node.Color = Color.MediumPurple;
            }

            return graph;
        }

        public static Layout FourGiantsLayout()
        {
            var graph = FourGiantsGraph();
            var templateGroups = FourGiantsTemplateGroups();
            var generator = new LayoutGenerator(12345, graph, templateGroups);
            return generator.GenerateLayout();
        }
    }
}
