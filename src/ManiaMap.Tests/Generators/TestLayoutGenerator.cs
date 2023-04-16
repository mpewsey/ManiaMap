using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Logging;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestLayoutGenerator
    {
        [TestMethod]
        public void TestCancellationToken()
        {
            var log = new List<string>();
            Logger.RemoveAllListeners();
            Logger.AddListener(log.Add);
            Logger.AddListener(Console.WriteLine);

            var token = new CancellationTokenSource(20).Token;
            var random = new RandomSeed(12345);
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();
            var generator = new LayoutGenerator();
            var layout = generator.Generate(1, graph, templateGroups, random, token);

            Logger.RemoveAllListeners();
            Assert.IsTrue(log.Count > 0);
            Assert.IsTrue(log[log.Count - 1].Contains("Process cancelled"));
            Assert.IsNull(layout);
        }

        [TestMethod]
        public void TestHyperSquareCrossLayout()
        {
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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
            Logger.RemoveAllListeners();
            Logger.AddListener(Console.WriteLine);

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