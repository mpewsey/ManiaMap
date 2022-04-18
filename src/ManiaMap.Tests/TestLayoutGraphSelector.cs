using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGraphSelector
    {
        [TestMethod]
        public void TestListSelector()
        {
            var seed = new RandomSeed(12345);

            var graphs = new List<LayoutGraph>()
            {
                new LayoutGraph(1, "Graph1"),
                new LayoutGraph(2, "Graph2"),
                new LayoutGraph(3, "Graph3"),
            };

            var input = new Dictionary<string, object>
            {
                { "LayoutGraphs", graphs },
                { "RandomSeed", seed },
            };

            var selector = new LayoutGraphSelector();
            var results = new GenerationPipeline.Results(input);
            selector.ApplyStep(results);
            Assert.IsTrue(results.Success);
            Assert.AreEqual(1, results.Outputs.Count);
            Assert.IsTrue(results.Outputs.ContainsKey("LayoutGraph"));
        }

        [TestMethod]
        public void TestFunctionSelector()
        {
            var seed = new RandomSeed(12345);

            var graphs = new List<Func<LayoutGraph>>()
            {
                () => new LayoutGraph(1, "Graph1"),
                () => new LayoutGraph(2, "Graph2"),
                () => new LayoutGraph(3, "Graph3"),
            };

            var input = new Dictionary<string, object>
            {
                { "LayoutGraphs", graphs },
                { "RandomSeed", seed },
            };

            var selector = new LayoutGraphSelector();
            var results = new GenerationPipeline.Results(input);
            selector.ApplyStep(results);
            Assert.IsTrue(results.Success);
            Assert.AreEqual(1, results.Outputs.Count);
            Assert.IsTrue(results.Outputs.ContainsKey("LayoutGraph"));
        }
    }
}