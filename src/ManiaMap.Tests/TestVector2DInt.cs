using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestVector2DInt
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(1, 2);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(1, 4);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(1, 2);
            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void TestInitializers()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(1, 2);
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
        }

        [TestMethod]
        public void TestAddOperator()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(9, 4);
            var expected = new Vector2DInt(10, 6);
            Assert.AreEqual(expected, x + y);
        }

        [TestMethod]
        public void TestSubtractOperator()
        {
            var x = new Vector2DInt(1, 2);
            var y = new Vector2DInt(9, 4);
            var expected = new Vector2DInt(-8, -2);
            Assert.AreEqual(expected, x - y);
        }
    }
}