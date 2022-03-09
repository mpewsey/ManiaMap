using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGraphCycleDecomposer
    {
        [TestMethod]
        public void TestFindCyclesOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
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
                CollectionAssert.AreEquivalent(expected[i], cycles[i]);
            }
        }

        [TestMethod]
        public void TestFindCyclesOfLoopGraph()
        {
            var graph = Samples.GraphLibrary.LoopGraph();
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
                CollectionAssert.AreEquivalent(expected[i], cycles[i]);
            }
        }

        [TestMethod]
        public void TestToString()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var decomposer = new GraphCycleDecomposer(graph);
            Assert.IsTrue(decomposer.ToString().StartsWith("GraphCycleDecomposer("));
        }
    }
}