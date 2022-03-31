using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectable
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Collectable("Default", 1);
            var y = new Collectable("Default", 1);
            Assert.IsTrue(x == y);
            Assert.IsFalse(x.Equals(null));
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Collectable("Default", 1);
            var y = new Collectable("Default", 2);
            Assert.IsTrue(x != y);
        }
    }
}