using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
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
            var expected1 = new List<RoomTemplate> { template1 };
            var groups = new TemplateGroups();

            groups.Add("Group1", template1);
            CollectionAssert.AreEquivalent(expected1, groups.GroupsDictionary["Group1"]);

            groups.Add("Group2", expected1);
            CollectionAssert.AreEquivalent(expected1, groups.GroupsDictionary["Group2"]);
        }

        [TestMethod]
        public void TestSaveAndLoadXml()
        {
            var path = "TemplateGroups.xml";
            var groups = Samples.ManiaMapSample.LetterTemplateGroups();
            XmlSerialization.SaveXml(path, groups);
            var copy = XmlSerialization.LoadXml<TemplateGroups>(path);
            CollectionAssert.AreEquivalent(groups.GroupsDictionary.Keys.ToList(), copy.GroupsDictionary.Keys.ToList());

            foreach (var pair in groups.GroupsDictionary)
            {
                var x = pair.Value.Select(x => x.Id).ToList();
                var y = copy.GroupsDictionary[pair.Key].Select(x => x.Id).ToList();
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