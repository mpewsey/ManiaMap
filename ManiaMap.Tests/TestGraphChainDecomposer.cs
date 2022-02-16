using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGraphChainDecomposer
    {
        [TestMethod]
        public void TestFindChainsOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var chains = graph.FindChains();

            var expected = new List<List<LayoutEdge>>
            {
                new() { graph.GetEdge(11, 13), graph.GetEdge(11, 12), graph.GetEdge(12, 13) },
                new() { graph.GetEdge(10, 11), graph.GetEdge(6, 10) },
                new() { graph.GetEdge(4, 6), graph.GetEdge(5, 6), graph.GetEdge(3, 5), graph.GetEdge(3, 4) },
                new() { graph.GetEdge(5, 9) },
                new() { graph.GetEdge(4, 7), graph.GetEdge(7, 8) },
                new() { graph.GetEdge(2, 3), graph.GetEdge(1, 2) },

            };

            Console.WriteLine("Expected:");
            expected.ForEach(x => Console.WriteLine(string.Join(", ", x.Select(x => x.ToShortString()))));

            Console.WriteLine("\nResult:");
            chains.ForEach(x => Console.WriteLine(string.Join(", ", x.Select(x => x.ToShortString()))));

            Assert.AreEqual(expected.Count, chains.Count);

            for (int i = 0; i < chains.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], chains[i]);
            }
        }

        [TestMethod]
        public void TestFindChainsOfLoopGraph()
        {
            var graph = Samples.GraphLibrary.LoopGraph();
            var chains = graph.FindChains();

            var expected = new List<List<LayoutEdge>>
            {
                new() { graph.GetEdge(4, 0), graph.GetEdge(0, 1), graph.GetEdge(1, 2), graph.GetEdge(2, 3), graph.GetEdge(3, 4) },
                new() { graph.GetEdge(0, 5) },
                new() { graph.GetEdge(7, 3), graph.GetEdge(6, 7), graph.GetEdge(5, 6) },
            };

            Console.WriteLine("Expected:");
            expected.ForEach(x => Console.WriteLine(string.Join(", ", x.Select(x => x.ToShortString()))));

            Console.WriteLine("\nResult:");
            chains.ForEach(x => Console.WriteLine(string.Join(", ", x.Select(x => x.ToShortString()))));

            Assert.AreEqual(expected.Count, chains.Count);

            for (int i = 0; i < chains.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], chains[i]);
            }
        }
    }
}