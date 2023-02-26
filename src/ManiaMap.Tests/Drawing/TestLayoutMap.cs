using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Generators;
using System;

namespace MPewsey.ManiaMap.Drawing.Tests
{
    [TestClass]
    public class TestLayoutMap
    {
        [TestMethod]
        public void TestSaveManiaMapImages()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoors:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            var map = new LayoutMap(new Padding(4));
            map.SaveImages("ManiaMap.png", layout);
        }

        [TestMethod]
        public void TestSaveHyperSquareStackedLoopLayoutImages()
        {
            var graph = Samples.GraphLibrary.StackedLoopGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap();
            map.SaveImages("HyperSquareMap.png", layout);
        }

        [TestMethod]
        public void TestSaveHyperSquareStackedLoopLayoutWithoutDoorsImages()
        {
            var graph = Samples.GraphLibrary.StackedLoopGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(doorDrawMode: DoorDrawMode.None);
            map.SaveImages("HyperSquareMapNoDoors.png", layout);
        }

        [TestMethod]
        public void TestSaveLLoopLayoutImages()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap();
            map.SaveImages("LLoopMap.png", layout);
        }

        [TestMethod]
        public void TestSaveLLoopLayoutWithoutDoorsImages()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(doorDrawMode: DoorDrawMode.None);
            map.SaveImages("LLoopMapNoDoors.png", layout);
        }

        [TestMethod]
        public void TestToString()
        {
            var map = new LayoutMap();
            Assert.IsTrue(map.ToString().StartsWith("LayoutMap("));
        }

        [TestMethod]
        public void TestSaveFilteredVisibleLLoopLayoutImages()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var layoutState = new LayoutState(layout);

            foreach (var roomState in layoutState.RoomStates.Values)
            {
                roomState.IsVisible = true;
                var cells = layout.Rooms[roomState.Id].Template.Cells;

                for (int i = 0; i < cells.Rows; i++)
                {
                    for (int j = 0; j < cells.Columns; j++)
                    {
                        var index = new Vector2DInt(i, j);
                        var visibility = random.ChanceSatisfied(0.5);
                        Assert.IsTrue(roomState.SetCellVisibility(index, visibility));
                    }
                }
            }

            var map = new LayoutMap();
            map.SaveImages("FilteredVisibleLLoopMap.png", layout, layoutState);
        }

        [TestMethod]
        public void TestSaveFilteredLLoopLayoutImages()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var layoutState = new LayoutState(layout);

            foreach (var roomState in layoutState.RoomStates.Values)
            {
                var cells = layout.Rooms[roomState.Id].Template.Cells;

                for (int i = 0; i < cells.Rows; i++)
                {
                    for (int j = 0; j < cells.Columns; j++)
                    {
                        var index = new Vector2DInt(i, j);
                        var visibility = random.ChanceSatisfied(0.5);
                        Assert.IsTrue(roomState.SetCellVisibility(index, visibility));
                    }
                }
            }

            var map = new LayoutMap();
            map.SaveImages("FilteredLLoopMap.png", layout, layoutState);
        }

        [TestMethod]
        public void TestSaveBigLayoutImages()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap();
            map.SaveImages("BigLayoutMap.png", layout);
        }

        [TestMethod]
        public void TestDirectedRingLayout()
        {
            var graph = Samples.GraphLibrary.DirectedRingGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Nodes", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());
            templateGroups.Add("Edges", Samples.TemplateLibrary.Squares.Square1x1NorthExitTemplate().AllVariations());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            var map = new LayoutMap();
            map.SaveImages("DirectedRingLayout.png", layout);
        }
    }
}
