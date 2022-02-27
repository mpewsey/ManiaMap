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
            var layout = generator.GenerateLayout();

            Assert.IsNotNull(layout);

            var map = new LayoutMap(layout);
            map.SaveImage("HyperSquareMap.png");
        }

        [TestMethod]
        public void TestSaveLLoopLayoutImage()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout();

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
    }
}
