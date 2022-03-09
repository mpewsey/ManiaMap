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

        [TestMethod]
        public void TestCompareTo()
        {
            var edge1 = new EdgeIndexes(1, 2);
            var edge2 = new EdgeIndexes(1, 3);
            var edge3 = new EdgeIndexes(2, 2);
            Assert.AreEqual(-1, edge1.CompareTo(edge2));
            Assert.AreEqual(1, edge2.CompareTo(edge1));
            Assert.AreEqual(-1, edge1.CompareTo(edge3));
            Assert.AreEqual(1, edge3.CompareTo(edge1));
        }
    }
}