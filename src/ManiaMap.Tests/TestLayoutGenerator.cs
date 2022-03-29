using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGenerator
    {
        [TestMethod]
        public void TestHyperSquareCrossLayout()
        {
            var graph = Samples.GraphLibrary.CrossGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(graph, templateGroups);
            var random = new RandomSeed(12345);
            var layout = generator.GenerateLayout(1, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestHyperSquareStackedLoopLayout()
        {
            var graph = Samples.GraphLibrary.StackedLoopGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(graph, templateGroups);
            var random = new RandomSeed(12345);
            var layout = generator.GenerateLayout(1, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestLLoopLayout()
        {
            var graph = Samples.GraphLibrary.LoopGraph();

            var templateGroups = new TemplateGroups();
            var template = Samples.TemplateLibrary.Miscellaneous.LTemplate();
            templateGroups.Add("Default", template.UniqueVariations());

            var generator = new LayoutGenerator(graph, templateGroups);
            var random = new RandomSeed(12345);
            var layout = generator.GenerateLayout(1, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoor Connections:");
            layout.DoorConnections.ForEach(x => Console.WriteLine(x));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestHyperSquareGeekLayout()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(graph, templateGroups);
            var random = new RandomSeed(123456);
            var layout = generator.GenerateLayout(1, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestManiaMapLayout()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoor Connections:");
            layout.DoorConnections.ForEach(x => Console.WriteLine(x));

            Assert.AreEqual(8, layout.Rooms.Count);
            Assert.AreEqual(7, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestToString()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(graph, templateGroups);
            Assert.IsTrue(generator.ToString().StartsWith("LayoutGenerator("));
        }
    }
}