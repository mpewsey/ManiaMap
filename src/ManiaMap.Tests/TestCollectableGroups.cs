using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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

            groups.Groups.Clear();
            groups.Add("Default", 1);
            groups.Add("Default", 2);
            groups.Add("Default", 3);
            CollectionAssert.AreEqual(expected, groups.Groups["Default"]);

            groups.Groups.Clear();
            groups.Add("Default", 1, 2, 3);
            CollectionAssert.AreEqual(expected, groups.Groups["Default"]);

            groups.Groups.Clear();
            groups.Add("Default", new List<int> { 1, 2, 3 });
            CollectionAssert.AreEqual(expected, groups.Groups["Default"]);

            groups.Groups.Clear();
            groups.Add("Default", new List<int> { 1, 2 }, new List<int> { 3 });
            CollectionAssert.AreEqual(expected, groups.Groups["Default"]);
        }

        [TestMethod]
        public void TestGetCollectables()
        {
            var groups = new CollectableGroups();
            groups.Add("Default", 1);
            var expected = new List<Collectable> { new Collectable("Default", 1) };
            var result = groups.GetCollectables().ToList();
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToString()
        {
            var groups = new CollectableGroups();
            Assert.IsTrue(groups.ToString().StartsWith("CollectableGroups("));
        }
    }
}