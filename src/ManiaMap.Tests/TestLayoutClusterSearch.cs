using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutClusterSearch
    {
        [TestMethod]
        public void TestFindClusterOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(123456, graph, templateGroups);
            var layout = generator.GenerateLayout(1);

            var result = layout.FindCluster(new Uid(5), 2);
            var expected = new List<int>() { 5, 3, 2, 4, 6, 9, 10 };
            var values = expected.Select(x => new Uid(x)).ToList();
            CollectionAssert.AreEquivalent(values, result);
        }
    }
}