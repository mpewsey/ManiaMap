using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestEdgeIndexes
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new EdgeIndexes(1, 2);
            var y = new EdgeIndexes(1, 2);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new EdgeIndexes(1, 2);
            var y = new EdgeIndexes(2, 1);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var x = new EdgeIndexes(1, 2);
            var y = new EdgeIndexes(1, 2);
            Assert.IsTrue(x.Equals((object)y));
        }

        [TestMethod]
        public void TestToString()
        {
            var result = new EdgeIndexes(1, 2).ToString();
            var expected = "EdgeIndexes(FromIndex = 1, ToIndex = 2)";
            Assert.AreEqual(expected, result);
        }
    }
}