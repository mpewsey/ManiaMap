using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGraphBranchDecomposer
    {
        [TestMethod]
        public void TestFindBranchesOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var branches = graph.FindBranches();

            var expected = new List<List<int>>
            {
                new() { 9, 5 },
                new() { 1, 2, 3 },
                new() { 8, 7, 4 },
                new() { 11, 10, 6 },
            };

            Console.WriteLine("Expected:");
            expected.ForEach(x => Console.WriteLine(string.Join(", ", x)));

            Console.WriteLine("\nResult:");
            branches.ForEach(x => Console.WriteLine(string.Join(", ", x)));

            branches.Sort((x, y) => x.Count.CompareTo(y.Count));
            expected.Sort((x, y) => x.Count.CompareTo(y.Count));
            Assert.AreEqual(expected.Count, branches.Count);

            for (int i = 0; i < branches.Count; i++)
            {
                CollectionAssert.AreEquivalent(expected[i], branches[i]);
            }
        }

        [TestMethod]
        public void TestToString()
        {
            var graph = Samples.GraphLibrary.GeekGraph();
            var decomposer = new GraphBranchDecomposer(graph);
            Assert.IsTrue(decomposer.ToString().StartsWith("GraphBranchDecomposer("));
        }
    }
}