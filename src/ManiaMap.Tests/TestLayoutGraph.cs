using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}