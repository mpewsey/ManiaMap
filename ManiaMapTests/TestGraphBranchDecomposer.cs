using ManiaMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace ManiaMapTests
{
    [TestClass]
    public class TestGraphBranchDecomposer
    {
        private static LayoutGraph GetTestGraph1()
        {
            var graph = new LayoutGraph();

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

        private static void PrintBranches(List<List<int>> branches)
        {
            for (int i = 0; i < branches.Count; i++)
            {
                Debug.WriteLine($"Branch {i}: {string.Join(", ", branches[i])}");
            }
        }

        private static void TestBranchEquality(List<List<int>> expected, List<List<int>> branches)
        {
            Debug.WriteLine("\nExpected:");
            PrintBranches(expected);
            Debug.WriteLine("\nResult:");
            PrintBranches(branches);

            branches.Sort((x, y) => x.Count.CompareTo(y.Count));
            expected.Sort((x, y) => x.Count.CompareTo(y.Count));
            Assert.AreEqual(expected.Count, branches.Count);

            for (int i = 0; i < branches.Count; i++)
            {
                branches[i].Sort();
                expected[i].Sort();
                CollectionAssert.AreEqual(expected[i], branches[i]);
            }
        }

        [TestMethod]
        public void TestFindBranches1()
        {
            var graph = GetTestGraph1();
            var branches = GraphBranchDecomposer.FindBranches(graph);

            var expected = new List<List<int>>
            {
                new() { 9, 5 },
                new() { 8, 7, 4 },
                new() { 1, 2, 3 },
                new() { 11, 10, 6 },
            };

            TestBranchEquality(expected, branches);
        }
    }
}