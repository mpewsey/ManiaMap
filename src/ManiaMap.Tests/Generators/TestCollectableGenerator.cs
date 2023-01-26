using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestCollectableGenerator
    {
        [TestMethod]
        public void TestGenerateLayoutForBigLayout()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();

            var collectableGroups = new CollectableGroups();
            var expected = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            collectableGroups.Add("Default", expected);

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);
            Assert.IsNotNull(layout);

            var collectableGenerator = new CollectableGenerator();
            collectableGenerator.Generate(layout, collectableGroups, random);
            var result = new List<int>();

            foreach (var room in layout.Rooms.Values)
            {
                result.AddRange(room.Collectables.Values);
            }

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void TestCollectableSpotNotFound()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = Samples.BigLayoutSample.BigLayoutTemplateGroups();

            var collectableGroups = new CollectableGroups();
            var expected = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            collectableGroups.Add("NotInGraph", expected);

            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);
            Assert.IsNotNull(layout);

            var collectableGenerator = new CollectableGenerator();
            Assert.ThrowsException<CollectableSpotNotFoundException>(() => collectableGenerator.Generate(layout, collectableGroups, random));
        }

        [TestMethod]
        public void TestToString()
        {
            var generator = new CollectableGenerator();
            Assert.IsTrue(generator.ToString().StartsWith("CollectableGenerator("));
        }
    }
}