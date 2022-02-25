using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutNeighborSearch
    {
        [TestMethod]
        public void TestFindNeighborsOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator(123456, graph, templateGroups);
            var layout = generator.GenerateLayout();

            var result = layout.FindNeighbors(5, 2);
            var expected = new List<RoomId>() { 5, 3, 2, 4, 6, 9, 10 };
            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}