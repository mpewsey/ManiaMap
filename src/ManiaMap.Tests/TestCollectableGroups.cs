using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectableGroups
    {
        [TestMethod]
        public void TestAdd()
        {
            var groups = new CollectableGroups();
            var expected = new List<int> { 1, 2, 3 };

            groups.Add("Default", 1);
            groups.Add("Default", 2);
            groups.Add("Default", 3);
            CollectionAssert.AreEqual(expected, groups.GroupsDictionary["Default"]);
        }

        [TestMethod]
        public void TestAddMultiple()
        {
            var expected = new List<int> { 1, 2, 3 };
            var groups = new CollectableGroups();
            groups.Add("Default", new List<int> { 1, 2, 3 });
            CollectionAssert.AreEqual(expected, groups.GroupsDictionary["Default"]);
        }

        [TestMethod]
        public void TestToString()
        {
            var groups = new CollectableGroups();
            Assert.IsTrue(groups.ToString().StartsWith("CollectableGroups("));
        }

        [TestMethod]
        public void TestInvalidGroupName()
        {
            var group = new CollectableGroups();
            Assert.ThrowsException<InvalidNameException>(() => group.Add("", 1));
        }

        [TestMethod]
        public void TestSaveAndLoad()
        {
            var group = new CollectableGroups();
            group.Add("Group1", new int[] { 1, 2, 3, 4 });
            group.Add("Group2", new int[] { 5, 6, 7, 8 });
            var path = "CollectableGroups.xml";
            XmlSerialization.SaveXml(path, group);
            var copy = XmlSerialization.LoadXml<CollectableGroups>(path);

            foreach (var pair in group.GroupsDictionary)
            {
                CollectionAssert.AreEqual(pair.Value, copy.GroupsDictionary[pair.Key]);
            }
        }
    }
}