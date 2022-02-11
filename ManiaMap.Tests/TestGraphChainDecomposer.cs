using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGraphChainDecomposer
    {
        private static LayoutGraph GetGeekGraph()
        {
            var graph = new LayoutGraph(1);

            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 6);
            graph.AddEdge(4, 7);
            graph.AddEdge(5, 6);
            graph.AddEdge(3, 5);
            graph.AddEdge(7, 8);
            graph.AddEdge(6, 10);
            graph.AddEdge(5, 9);
            graph.AddEdge(10, 11);
            graph.AddEdge(11, 12);
            graph.AddEdge(11, 13);
            graph.AddEdge(12, 13);

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

            return graph;
        }

        [TestMethod]
        public void TestFindChainsOfGeekGraph()
        {
            var graph = GetGeekGraph();
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
            var graph = GetLoopGraph();
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