using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGraph
    {
        [TestMethod]
        public void TestAddRandomEdges()
        {
            var random = new Random(1);
            var graph = new LayoutGraph(1);
            graph.AddRandomEdges(0, 40, 100, random);

            Console.WriteLine("Edges:");
            Console.WriteLine(string.Join("\n", graph.GetEdges().Select(x => x.ToShortString())));
        }
    }
}
