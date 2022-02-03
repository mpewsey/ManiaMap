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
        
        [TestMethod]
        public void TestGetCycleNodes()
        {
            var graph = GetTestGraph1();

            var expected = new List<int>[]
            {
                new() { 3, 4, 5, 6 },
                new() { 11, 12, 13 },
            };

            var cycles = graph.GetCycleNodes();
            Assert.AreEqual(expected.Length, cycles.Count);

            for (int i = 0; i < expected.Length; i++)
            {
                cycles[i].Sort();
                expected[i].Sort();
                Debug.WriteLine($"Cycle {i}: {string.Join(", ", cycles[i])}");
                CollectionAssert.AreEqual(expected[i], cycles[i]);
            }
        }
    }
}