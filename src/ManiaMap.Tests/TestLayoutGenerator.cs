using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;
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

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

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

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

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

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections.Values));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestHyperSquareGeekLayout()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(123456);
            var layout = generator.Generate(1, graph, templateGroups, random);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestGraphIsNotFullyConnected()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            graph.AddNode(100000);

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(123456);
            Assert.ThrowsException<GraphNotFullyConnectedException>(() => generator.Generate(1, graph, templateGroups, random));
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
            Console.WriteLine(string.Join("\n", layout.DoorConnections.Values));

            Assert.AreEqual(8, layout.Rooms.Count);
            Assert.AreEqual(7, layout.DoorConnections.Count);
        }

        [TestMethod]
        public void TestToString()
        {
            var generator = new LayoutGenerator();
            Assert.IsTrue(generator.ToString().StartsWith("LayoutGenerator("));
        }

        [DataTestMethod]
        [DataRow(12345)]
        [DataRow(12355)]
        [DataRow(12365)]
        [DataRow(12375)]
        [DataRow(12385)]
        [DataRow(123456789)]
        public void TestBigLayout(int seed)
        {
            var random = new RandomSeed(seed);
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();
            var generator = new LayoutGenerator();
            var layout = generator.Generate(1, graph, templateGroups, random);
            Assert.IsNotNull(layout);
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

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount + 2, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount + 2, layout.DoorConnections.Count);
        }
    }
}