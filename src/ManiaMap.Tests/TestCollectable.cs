using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectable
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Collectable("Group1", 1);
            var y = new Collectable("Group1", 1);
            Assert.IsTrue(x == y);
            Assert.IsFalse(x.Equals(null));
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Collectable("Group1", 1);
            var y = new Collectable("Group2", 2);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestToString()
        {
            var x = new Collectable("Group1", 1);
            Assert.AreEqual("Collectable(Group = Group1, Id = 1)", x.ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var x = new Collectable("Group1", 1);
            var y = new Collectable("Group1", 1);
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
        }
    }
}