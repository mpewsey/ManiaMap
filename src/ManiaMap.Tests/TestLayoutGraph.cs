using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGraph
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "LayoutGraphSaveAndLoad.xml";
            var graph = Samples.GraphLibrary.BigGraph();
            graph.AddNodeVariation("Variation1", new int[] { 1, 2, 3 });
            XmlSerialization.SaveXml(path, graph);
            var copy = XmlSerialization.LoadXml<LayoutGraph>(path);
            Assert.AreEqual(graph.Id, copy.Id);
            Assert.AreEqual(graph.Name, copy.Name);
            Assert.AreEqual(graph.NodeCount, copy.NodeCount);
            Assert.AreEqual(graph.EdgeCount, copy.EdgeCount);

            CollectionAssert.AreEquivalent(
                graph.GetNodes().Select(x => x.Id).ToList(),
                copy.GetNodes().Select(x => x.Id).ToList());

            CollectionAssert.AreEquivalent(
                graph.GetEdges().Select(x => new EdgeIndexes(x.FromNode, x.ToNode)).ToList(),
                copy.GetEdges().Select(x => new EdgeIndexes(x.FromNode, x.ToNode)).ToList());

            foreach (var node in graph.GetNodes())
            {
                CollectionAssert.AreEqual(
                    graph.GetNeighbors(node.Id).ToList(),
                    copy.GetNeighbors(node.Id).ToList());
            }

            var dict1 = new Dictionary<string, List<int>>(graph.GetNodeVariations());
            var dict2 = new Dictionary<string, List<int>>(copy.GetNodeVariations());
            Assert.AreEqual(dict1.Count, dict2.Count);

            foreach (var pair in dict1)
            {
                CollectionAssert.AreEqual(pair.Value, dict2[pair.Key]);
            }
        }

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
        public void TestSaveAndLoadXml()
        {
            var path = "LayoutGraph.xml";
            var graph = Samples.GraphLibrary.GeekGraph();
            XmlSerialization.SaveXml(path, graph);
            var copy = XmlSerialization.LoadXml<LayoutGraph>(path);
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

        [TestMethod]
        public void TestAddNodeVariation()
        {
            var graph = new LayoutGraph(1, "Variations");
            graph.AddNodeVariation("Group1", 1);
            graph.AddNodeVariation("Group1", 2);
            var expected = new List<int> { 1, 2 };
            CollectionAssert.AreEquivalent(expected, graph.GetNodeVariations("Group1").ToList());
            CollectionAssert.AreEquivalent(expected, graph.GetNodes().Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void TestAddNodeVariations()
        {
            var graph = new LayoutGraph(1, "Variations");
            graph.AddNodeVariation("Group1", new int[] { 1, 2 });
            var expected = new List<int> { 1, 2 };
            CollectionAssert.AreEquivalent(expected, graph.GetNodeVariations("Group1").ToList());
            CollectionAssert.AreEquivalent(expected, graph.GetNodes().Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void TestRemoveAllVariations()
        {
            var graph = new LayoutGraph(1, "Variations");
            graph.AddNodeVariation("Group1", 1);
            graph.RemoveNodeVariations(1);
            CollectionAssert.AreEquivalent(new List<int>(), graph.GetNodeVariations("Group1").ToList());
        }

        [TestMethod]
        public void TestRemoveNodeVariation()
        {
            var graph = new LayoutGraph(1, "Variations");
            graph.AddNodeVariation("Group1", 1);
            graph.RemoveNodeVariation("Group1", 1);
            CollectionAssert.AreEquivalent(new List<int>(), graph.GetNodeVariations("Group1").ToList());
            CollectionAssert.AreEquivalent(new List<int> { 1 }, graph.GetNodes().Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void TestRemoveNodeVariations()
        {
            var graph = new LayoutGraph(1, "Variations");
            graph.AddNodeVariation("Group1", new int[] { 1, 2 });
            graph.RemoveNodeVariation("Group1", new int[] { 1, 2 });
            var expected = new List<int> { 1, 2 };
            CollectionAssert.AreEquivalent(new List<int>(), graph.GetNodeVariations("Group1").ToList());
            CollectionAssert.AreEquivalent(expected, graph.GetNodes().Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void TestGetVariation()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.AddNodeVariation("Group1", new int[] { 0, 1 });
            var seed = new RandomSeed(12345);

            for (int i = 0; i < 1000; i++)
            {
                var variation = graph.GetVariation(seed);
                Assert.AreEqual(graph.Id, variation.Id);
                Assert.AreEqual(graph.Name, variation.Name);
            }
        }

        [TestMethod]
        public void TestAddAddBetweenSameNodes()
        {
            var graph = new LayoutGraph(1, "Test");
            Assert.ThrowsException<InvalidIdException>(() => graph.AddEdge(1, 1));
        }

        [TestMethod]
        public void TestFindCluster()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var result = graph.FindCluster(5, 2).ToList();
            var expected = new List<int>() { 5, 3, 2, 4, 6, 9, 10 };
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void TestFindClusters()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var result = graph.FindClusters(2);
            var expected = new List<int>() { 5, 3, 2, 4, 6, 9, 10 };
            CollectionAssert.AreEquivalent(expected, result[5].ToList());
        }

        [TestMethod]
        public void TestEmptyGraphIsFullyConnected()
        {
            var graph = new LayoutGraph(1, "Test");
            Assert.IsTrue(graph.IsFullyConnected());
        }

        [TestMethod]
        public void TestIsFullyConnected()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            Assert.IsTrue(graph.IsFullyConnected());
        }

        [TestMethod]
        public void TestIsNotFullyConnected()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            graph.AddNode(10000);
            Assert.IsFalse(graph.IsFullyConnected());
        }

        [TestMethod]
        public void TestIsValid()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            Assert.IsTrue(graph.IsValid());
        }
    }
}