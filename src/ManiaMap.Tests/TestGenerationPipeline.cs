using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGenerationPipeline
    {
        [TestMethod]
        public void TestDefaultPipelineGeneration()
        {
            var random = new RandomSeed(12345);
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();
            var collectableGroups = new CollectableGroups();
            collectableGroups.Add("Default", new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            var args = new Dictionary<string, object>
            {
                { "LayoutId", 1 },
                { "LayoutGraph", graph },
                { "TemplateGroups", templateGroups },
                { "CollectableGroups", collectableGroups },
                { "RandomSeed", random },
            };

            var pipeline = GenerationPipeline.CreateDefaultPipeline();
            var results = pipeline.Generate(args);
            Assert.IsTrue(results.ContainsKey("Layout"));
            Assert.IsNotNull(results["Layout"] as Layout);
        }
    }
}