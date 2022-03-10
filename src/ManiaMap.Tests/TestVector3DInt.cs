using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestVector3DInt
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(1, 2, 3);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(1, 4, 3);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(1, 2, 3);
            Assert.IsTrue(x.Equals(y));
        }

        [TestMethod]
        public void TestInitializers()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(1, 2, 3);
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
        }

        [TestMethod]
        public void TestAddOperator()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(9, 4, 2);
            var expected = new Vector3DInt(10, 6, 5);
            Assert.AreEqual(expected, x + y);
        }

        [TestMethod]
        public void TestSubtractOperator()
        {
            var x = new Vector3DInt(1, 2, 3);
            var y = new Vector3DInt(9, 4, 2);
            var expected = new Vector3DInt(-8, -2, 1);
            Assert.AreEqual(expected, x - y);
        }
    }
}