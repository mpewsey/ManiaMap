using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGenerator
    {
        private static RoomTemplate GetSquareTemplate()
        {
            var o = new Cell();
            var a = new Cell { LeftDoor = new(DoorType.TwoWay), TopDoor = new(DoorType.TwoWay) };
            var b = new Cell { TopDoor = new(DoorType.TwoWay) };
            var c = new Cell { TopDoor = new(DoorType.TwoWay), RightDoor = new(DoorType.TwoWay) };
            var d = new Cell { LeftDoor = new(DoorType.TwoWay) };
            var e = new Cell { RightDoor = new(DoorType.TwoWay) };
            var f = new Cell { LeftDoor = new(DoorType.TwoWay), BottomDoor = new(DoorType.TwoWay) };
            var g = new Cell { BottomDoor = new(DoorType.TwoWay) };
            var h = new Cell { BottomDoor = new(DoorType.TwoWay), RightDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { a, b, c },
                { d, o, e },
                { f, g, h },
            };

            return new(1, cells);
        }

        private static RoomTemplate GetLTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { LeftDoor = new(DoorType.TwoWay), TopDoor = new(DoorType.TwoWay), RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { BottomDoor = new(DoorType.TwoWay), RightDoor = new(DoorType.TwoWay), TopDoor = new(DoorType.TwoWay) };
            var c = new Cell { BottomDoor = new(DoorType.TwoWay), LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { a, x, x },
                { o, x, x },
                { o, x, x },
                { c, o, b },
            };

            return new(2, cells);
        }

        private static LayoutGraph GetCrossGraph()
        {
            var graph = new LayoutGraph(1);

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

        private static LayoutGraph GetLoopGraph()
        {
            var graph = new LayoutGraph(1);

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

        [TestMethod]
        public void TestGenerateCrossLayout()
        {
            var graph = GetCrossGraph();
            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", GetSquareTemplate());
            var generator = new LayoutGenerator(1, graph, templateGroups);
            var layout = generator.GenerateLayout();
            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoor Connections:");
            layout.DoorConnections.ForEach(x => Console.WriteLine(x));

            Assert.AreEqual(graph.NodeCount(), layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount(), layout.DoorConnections.Count);

            var map = new LayoutMap(layout);
            map.SavePng("CrossMap.png");
        }

        [TestMethod]
        public void TestGenerateLoopLayout()
        {
            var graph = GetLoopGraph();
            var template = GetLTemplate();
            var templates = template.AllVariations();
            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", templates);
            var generator = new LayoutGenerator(1, graph, templateGroups);
            var layout = generator.GenerateLayout();
            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoor Connections:");
            layout.DoorConnections.ForEach(x => Console.WriteLine(x));

            Assert.AreEqual(graph.NodeCount(), layout.Rooms.Count);
            Assert.AreEqual(graph.EdgeCount(), layout.DoorConnections.Count);

            var map = new LayoutMap(layout);
            map.SavePng("LoopMap.png");
        }

        [TestMethod]
        public void TestSerialization()
        {
            var graph = GetCrossGraph();
            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", GetSquareTemplate());
            var generator = new LayoutGenerator(1, graph, templateGroups);
            var layout = generator.GenerateLayout();
            Assert.IsNotNull(layout);

            var path = "Layout.xml";
            var serializer = new DataContractSerializer(layout.GetType());

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, layout);
            }

            using (var stream = File.OpenRead(path))
            {
                var copy = (Layout)serializer.ReadObject(stream);
                Assert.AreEqual(layout.Seed, copy.Seed);
            }
        }
    }
}