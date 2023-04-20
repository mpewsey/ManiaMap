using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Pipelines;
using MPewsey.Common.Random;
using System.Collections.Generic;
using System.Threading;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestLayoutGraphRandomizer
    {
        [TestMethod]
        public void TestApplyStep()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.AddNodeVariations("Group1", new int[] { 0, 1 });
            var seed = new RandomSeed(12345);

            var input = new Dictionary<string, object>
            {
                { "LayoutGraph", graph },
                { "RandomSeed", seed },
            };

            var randomizer = new LayoutGraphRandomizer();
            var results = new PipelineResults(input);
            Assert.IsTrue(randomizer.ApplyStep(results, CancellationToken.None));
            Assert.IsTrue(results.Outputs.ContainsKey("LayoutGraph"));
        }

        [TestMethod]
        public void TestRandomizeLayout()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.AddNodeVariations("Group1", new int[] { 0, 1 });
            var seed = new RandomSeed(12345);
            var randomizer = new LayoutGraphRandomizer();
            var result = randomizer.RandomizeGraph(graph, seed);
            Assert.AreNotEqual(graph, result);
        }
    }
}