using ManiaMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ManiaMapTests
{
    [TestClass]
    public class TestGraphCycleDecomposer
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
        public void TestFindCyclesOfGeekGraph()
        {
            var graph = GetGeekGraph();
            var cycles = graph.FindCycles();

            var expected = new List<List<int>>
            {
                new() { 3, 4, 5, 6 },
                new() { 11, 12, 13 },
            };

            Console.WriteLine("Expected:");
            expected.ForEach(x => Console.WriteLine(string.Join(", ", x)));
            Console.WriteLine("\nResult:");
            cycles.ForEach(x => Console.WriteLine(string.Join(", ", x)));

            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));
            expected.Sort((x, y) => x.Count.CompareTo(y.Count));
            Assert.AreEqual(expected.Count, cycles.Count);

            for (int i = 0; i < cycles.Count; i++)
            {
                cycles[i].Sort();
                expected[i].Sort();
                CollectionAssert.AreEqual(expected[i], cycles[i]);
            }
        }

        [TestMethod]
        public void TestFindCyclesOfLoopGraph()
        {
            var graph = GetLoopGraph();
            var cycles = graph.FindCycles();

            var expected = new List<List<int>>
            {
                new() { 4, 3, 2, 1, 0 },
                new() { 5, 6, 7, 3, 4, 0 },
                new() { 3, 2, 1, 0, 5, 6, 7 },
            };

            Console.WriteLine("Expected:");
            expected.ForEach(x => Console.WriteLine(string.Join(", ", x)));
            Console.WriteLine("\nResult:");
            cycles.ForEach(x => Console.WriteLine(string.Join(", ", x)));

            cycles.Sort((x, y) => x.Count.CompareTo(y.Count));
            expected.Sort((x, y) => x.Count.CompareTo(y.Count));
            Assert.AreEqual(expected.Count, cycles.Count);

            for (int i = 0; i < cycles.Count; i++)
            {
                cycles[i].Sort();
                expected[i].Sort();
                CollectionAssert.AreEqual(expected[i], cycles[i]);
            }
        }
    }
}