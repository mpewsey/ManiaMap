using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestTemplateGroups
    {
        [TestMethod]
        public void TestToString()
        {
            var templateGroups = new TemplateGroups();
            var template = new RoomTemplate(1, "Template", new Array2D<Cell>());
            templateGroups.Add("Default", template);
            Assert.AreEqual("TemplateGroups(Groups.Count = 1)", templateGroups.ToString());
        }

        [TestMethod]
        public void TestAdd()
        {
            var template1 = new RoomTemplate(1, "Template1", new Array2D<Cell>());
            var template2 = new RoomTemplate(2, "Template2", new Array2D<Cell>());
            var expected1 = new List<RoomTemplate> { template1 };
            var expected2 = new List<RoomTemplate> { template1, template2 };
            var templateGroups = new TemplateGroups();

            templateGroups.Add("Group1", template1);
            CollectionAssert.AreEquivalent(expected1, templateGroups.Groups["Group1"]);

            templateGroups.Add("Group2", expected1);
            CollectionAssert.AreEquivalent(expected1, templateGroups.Groups["Group2"]);

            templateGroups.Add("Group3", template1, template2);
            CollectionAssert.AreEquivalent(expected2, templateGroups.Groups["Group3"]);

            var list1 = new List<RoomTemplate> { template1 };
            var list2 = new List<RoomTemplate> { template2 };
            templateGroups.Add("Group4", list1, list2);
            CollectionAssert.AreEquivalent(expected2, templateGroups.Groups["Group4"]);
        }
    }
}