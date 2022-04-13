using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGraph
    {
        [TestMethod]
        public void TestAddEdge()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(2, 3);
            Assert.AreEqual(2, graph.EdgeCount);
            var expected = new List<EdgeIndexes>() { new EdgeIndexes(1, 2), new EdgeIndexes(2, 3) };
            var result = graph.GetEdges().Select(x => new EdgeIndexes(x.FromNode, x.ToNode)).ToList();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void TestAddNode()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddNode(1);
            graph.AddNode(1);
            Assert.AreEqual(1, graph.NodeCount);
            var expected = new List<int>() { 1 };
            var result = graph.GetNodes().Select(x => x.Id).ToList();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void TestRemoveNode()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddEdge(1, 2);
            Assert.AreEqual(1, graph.EdgeCount);
            graph.RemoveNode(1);
            Assert.AreEqual(0, graph.EdgeCount);
            var expected = new List<int>() { 2 };
            var result = graph.GetNodes().Select(x => x.Id).ToList();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void TestRemoveEdge()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddEdge(1, 2);
            Assert.AreEqual(1, graph.EdgeCount);
            graph.RemoveEdge(2, 1);
            Assert.AreEqual(0, graph.EdgeCount);
        }

        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "LayoutGraph.xml";
            var graph = Samples.GraphLibrary.GeekGraph();
            Serialization.SaveXml(path, graph);
            var copy = Serialization.LoadXml<LayoutGraph>(path);
            Assert.AreEqual(graph.Id, copy.Id);
        }

        [TestMethod]
        public void TestToString()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            Assert.IsTrue(graph.ToString().StartsWith("LayoutGraph("));
        }

        [TestMethod]
        public void TestGetNeighbors()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.GetNeighbors(2);
            var expected = new List<int> { 1, 3 };
            CollectionAssert.AreEquivalent(expected, graph.GetNeighbors(2).ToList());
        }

        [TestMethod]
        public void TestCopy()
        {
            var graph = new LayoutGraph(1, "Test");
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            var copy = graph.Copy();

            Assert.AreEqual(graph.Id, copy.Id);
            Assert.AreEqual(graph.Name, copy.Name);
            Assert.AreEqual(graph.NodeCount, copy.NodeCount);
            Assert.AreEqual(graph.EdgeCount, copy.EdgeCount);
        }

        [TestMethod]
        public void TestSwapEdges()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.SwapEdges(1, 0);
            var expected = new LayoutGraph(1, "CrossLayoutGraph");

            expected.AddEdge(1, 0);
            expected.AddEdge(0, 2);
            expected.AddEdge(1, 3);
            expected.AddEdge(1, 4);
            expected.AddEdge(1, 5);

            var expectedEdges = expected.GetEdges().Select(x => new EdgeIndexes(x.FromNode, x.ToNode)).ToList();
            var resultEdges = graph.GetEdges().Select(x => new EdgeIndexes(x.FromNode, x.ToNode)).ToList();

            var expectedNodes = expected.GetNodes().Select(x => x.Id).ToList();
            var resultNodes = graph.GetNodes().Select(x => x.Id).ToList();

            Console.WriteLine("Expected Edges:");
            Console.WriteLine(string.Join("\n", expectedEdges));
            Console.WriteLine("\nResult Edges:");
            Console.WriteLine(string.Join("\n", resultEdges));

            Console.WriteLine("\nExpected Nodes:");
            Console.WriteLine(string.Join(", ", expectedNodes));
            Console.WriteLine("\nResult Nodes:");
            Console.WriteLine(string.Join(", ", resultNodes));

            CollectionAssert.AreEquivalent(expectedEdges, resultEdges);
            CollectionAssert.AreEquivalent(expectedNodes, resultNodes);
            CollectionAssert.AreEquivalent(expected.GetNeighbors(1).ToList(), graph.GetNeighbors(1).ToList());
            CollectionAssert.AreEquivalent(expected.GetNeighbors(0).ToList(), graph.GetNeighbors(0).ToList());
        }
    }
}