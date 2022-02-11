using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutMap
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

        private static LayoutGraph GetCrossGraph()
        {
            var graph = new LayoutGraph(1);

            graph.AddNode(0).TemplateGroups.Add("Default");
            graph.AddNode(1).TemplateGroups.Add("Default");
            graph.AddNode(2).TemplateGroups.Add("Default");
            graph.AddNode(3).TemplateGroups.Add("Default");
            graph.AddNode(4).TemplateGroups.Add("Default");
            graph.AddNode(5).TemplateGroups.Add("Default");

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(0, 3);
            graph.AddEdge(0, 4);
            graph.AddEdge(0, 5);

            return graph;
        }

        [TestMethod]
        public void TestSavePng()
        {
            var graph = GetCrossGraph();
            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", GetSquareTemplate());
            var generator = new LayoutGenerator(0, graph, templateGroups);
            var layout = generator.GenerateLayout();
            var map = new LayoutMap(layout);
            map.SavePng("Map.png");
        }
    }
}