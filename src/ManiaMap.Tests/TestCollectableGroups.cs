using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            CollectionAssert.AreEqual(expected, groups.Get("Default"));
        }

        [TestMethod]
        public void TestAddMultiple()
        {
            var expected = new List<int> { 1, 2, 3 };
            var groups = new CollectableGroups();
            groups.Add("Default", new List<int> { 1, 2, 3 });
            CollectionAssert.AreEqual(expected, groups.Get("Default"));
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
    }
}