using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;

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
            CollectionAssert.AreEquivalent(expected1, templateGroups.Get("Group1"));

            templateGroups.Add("Group2", expected1);
            CollectionAssert.AreEquivalent(expected1, templateGroups.Get("Group2"));
        }

        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "TemplateGroups.xml";
            var templateGroups = Samples.ManiaMapSample.LetterTemplateGroups();
            Serialization.SaveXml(path, templateGroups);
            var copy = Serialization.LoadXml<TemplateGroups>(path);
            CollectionAssert.AreEquivalent(templateGroups.GetGroupIds().ToList(), copy.GetGroupIds().ToList());

            foreach (var pair in templateGroups.GetGroups())
            {
                var x = pair.Value.Select(x => x.Id).ToList();
                var y = copy.Get(pair.Key).Select(x => x.Id).ToList();
                CollectionAssert.AreEquivalent(x, y);
            }
        }

        [TestMethod]
        public void TestInvalidGroupName()
        {
            var group = new TemplateGroups();
            var template = new RoomTemplate(1, "Test", new Array2D<Cell>());
            Assert.ThrowsException<InvalidNameException>(() => group.Add("", template));
        }
    }
}