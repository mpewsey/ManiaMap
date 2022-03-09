﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

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

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

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

            var generator = new LayoutGenerator(12345, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

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

            var generator = new LayoutGenerator(123456, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoor Connections:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            Assert.AreEqual(graph.NodeCount, layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount, layout.DoorConnections.Count);
        }

        [TestMethod]
        public async Task TestHyperSquareGeekAsyncLayout()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(123456, graph, templateGroups);
            var layout = await generator.GenerateLayoutAsync(1);

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

            var generator = new LayoutGenerator(123456, graph, templateGroups);
            Assert.IsTrue(generator.ToString().StartsWith("LayoutGenerator("));
        }
    }
}