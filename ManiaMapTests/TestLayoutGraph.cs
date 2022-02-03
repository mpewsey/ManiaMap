using ManiaMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace ManiaMapTests
{
    [TestClass]
    public class TestLayoutGraph
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

        private static LayoutGraph GetTestGraph2()
        {
            var graph = new LayoutGraph();

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
        public void TestGetCycles1()
        {
            var graph = GetTestGraph1();
            var cycles = graph.GetCycles();
            
            var expected = new List<List<int>>
            {
                new() { 3, 4, 5, 6 },
                new() { 11, 12, 13 },
            };

            for (int i = 0; i < cycles.Count; i++)
            {
                Debug.WriteLine($"Cycle {i}: {string.Join(", ", cycles[i])}");
            }

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
        public void TestGetCycles2()
        {
            var graph = GetTestGraph2();
            var cycles = graph.GetCycles();

            var expected = new List<List<int>>
            {
                new() { 4, 3, 2, 1, 0 },
                new() { 5, 6, 7, 3, 4, 0 },
                new() { 3, 2, 1, 0, 5, 6, 7 },
            };

            for (int i = 0; i < cycles.Count; i++)
            {
                Debug.WriteLine($"Cycle {i}: {string.Join(", ", cycles[i])}");
            }

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
        public void TestGetBranches1()
        {
            var graph = GetTestGraph1();
            var branches = graph.GetBranches();

            var expected = new List<List<int>>
            {
                new() { 9, 5 },
                new() { 8, 7, 4 },
                new() { 1, 2, 3 },
                new() { 11, 10, 6 },
            };

            for (int i = 0; i < branches.Count; i++)
            {
                Debug.WriteLine($"Branch {i}: {string.Join(", ", branches[i])}");
            }

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
        public void TestGetChains1()
        {
            var graph = GetTestGraph1();
            var chains = graph.GetChains();

            var expected = new List<List<int>>
            {

            };

            for (int i = 0; i < chains.Count; i++)
            {
                Debug.WriteLine($"Chain {i}: {string.Join(", ", chains[i])}");
            }

            chains.Sort((x, y) => x.Count.CompareTo(y.Count));
            expected.Sort((x, y) => x.Count.CompareTo(y.Count));
            Assert.AreEqual(expected.Count, chains.Count);

            for (int i = 0; i < chains.Count; i++)
            {
                chains[i].Sort();
                expected[i].Sort();
                CollectionAssert.AreEqual(expected[i], chains[i]);
            }
        }
    }
}