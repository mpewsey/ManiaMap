using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectable
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Collectable(1, "Group1");
            var y = new Collectable(1, "Group1");
            Assert.IsTrue(x == y);
            Assert.IsFalse(x.Equals(null));
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Collectable(1, "Group1");
            var y = new Collectable(2, "Group2");
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestToString()
        {
            var x = new Collectable(1, "Group1");
            Assert.AreEqual("Collectable(Id = 1, Group = Group1)", x.ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var x = new Collectable(1, "Group1");
            var y = new Collectable(1, "Group1");
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
        }
    }
}