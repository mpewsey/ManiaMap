using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutGraphRandomizer
    {
        [TestMethod]
        public void TestApplyStep()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.AddNodeVariation("Group1", new int[] { 0, 1 });
            var seed = new RandomSeed(12345);

            var input = new Dictionary<string, object>
            {
                { "LayoutGraph", graph },
                { "RandomSeed", seed },
            };

            var randomizer = new LayoutGraphRandomizer();
            var results = new GenerationPipeline.Results(input);
            randomizer.ApplyStep(results);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("LayoutGraph"));
        }

        [TestMethod]
        public void TestRandomizeLayout()
        {
            var graph = Samples.GraphLibrary.CrossGraph();
            graph.AddNodeVariation("Group1", new int[] { 0, 1 });
            var seed = new RandomSeed(12345);
            var randomizer = new LayoutGraphRandomizer();
            var result = randomizer.RandomizeGraph(graph, seed);
            Assert.AreNotEqual(graph, result);
        }
    }
}