using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestVector2DInt
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Vector2DInt.xml";
            var v = new Vector2DInt(1, 2);
            XmlSerialization.SaveXml(path, v);
            var copy = XmlSerialization.LoadXml<Vector2DInt>(path);
            Assert.AreEqual(v, copy);
        }

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