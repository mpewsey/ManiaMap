using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Drawing.Tests
{
    [TestClass]
    public class TestLayoutMap
    {
        [TestMethod]
        public void TestSaveManiaMapImage()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoors:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            var map = new LayoutMap(layout, padding: new Padding(4));
            map.SaveImage("ManiaMap.png");
        }

        [TestMethod]
        public void TestSaveHyperSquareStackedLoopLayoutImage()
        {
            var graph = Samples.GraphLibrary.StackedLoopGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(layout);
            map.SaveImage("HyperSquareMap.png");
        }

        [TestMethod]
        public void TestSaveHyperSquareStackedLoopLayoutImages()
        {
            var graph = Samples.GraphLibrary.StackedLoopGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(layout);
            map.SaveImages("HyperSquareMap.png");
        }

        [TestMethod]
        public void TestSaveLLoopLayoutImage()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(layout);
            map.SaveImage("LLoopMap.png");
        }

        [TestMethod]
        public void TestToString()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var map = new LayoutMap(layout);
            Assert.IsTrue(map.ToString().StartsWith("LayoutMap("));
        }

        [TestMethod]
        public void TestSaveFilteredLLoopLayoutImage()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            var layoutState = new LayoutState(layout);
            var random = new Random(12345);

            foreach (var roomState in layoutState.RoomStates.Values)
            {
                for (int i = 0; i < roomState.Visibility.Array.Length; i++)
                {
                    roomState.Visibility.Array[i] = random.NextDouble() > 0.5;
                }
            }

            var map = new LayoutMap(layout, layoutState);
            map.SaveImage("FilteredLLoopMap.png");
        }

        [TestMethod]
        public void TestSaveBigLayoutImage()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = new TemplateGroups();

            var square2x2 = Samples.TemplateLibrary.Squares.Square2x2Template();
            var square3x3 = Samples.TemplateLibrary.Squares.Square3x3Template();
            var rect2x3 = Samples.TemplateLibrary.Rectangles.Rectangle2x3Template();
            var rect2x4 = Samples.TemplateLibrary.Rectangles.Rectangle2x4Template();

            templateGroups.Add("Rooms",
                square2x2.UniqueVariations(),
                square3x3.UniqueVariations(),
                rect2x3.UniqueVariations(),
                rect2x4.UniqueVariations());

            var rect1x2 = Samples.TemplateLibrary.Rectangles.Rectangle1x2Template();
            var rect1x3 = Samples.TemplateLibrary.Rectangles.Rectangle1x3Template();
            var rect1x4 = Samples.TemplateLibrary.Rectangles.Rectangle1x4Template();
            var angle3x4 = Samples.TemplateLibrary.Angles.Angle3x4();

            templateGroups.Add("Paths",
                rect1x2.UniqueVariations(),
                rect1x3.UniqueVariations(),
                rect1x4.UniqueVariations(),
                angle3x4.UniqueVariations());

            var generator = new LayoutGenerator(123456, graph, templateGroups, 50);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            var map = new LayoutMap(layout);
            map.SaveImage("BigLayoutMap.png");
        }
    }
}
