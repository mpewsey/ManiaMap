using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRectangleInt
    {
        [TestMethod]
        public void TestToString()
        {
            var rect = new RectangleInt(1, 2, 3, 4);
            var expected = "RectangleInt(X = 1, Y = 2, Width = 3, Height = 4)";
            Assert.AreEqual(expected, rect.ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var rect1 = new RectangleInt(1, 2, 3, 4);
            var rect2 = new RectangleInt(5, 6, 7, 8);
            var rect3 = new RectangleInt(1, 2, 3, 4);
            Assert.AreEqual(rect1.GetHashCode(), rect3.GetHashCode());
            Assert.AreNotEqual(rect1.GetHashCode(), rect2.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var rect1 = new RectangleInt(1, 2, 3, 4);
            var rect2 = new RectangleInt(5, 6, 7, 8);
            var rect3 = new RectangleInt(1, 2, 3, 4);
            Assert.IsTrue(rect1 == rect3);
            Assert.IsFalse(rect1 == rect2);
            Assert.IsFalse(rect1 != rect3);
            Assert.IsTrue(rect1 != rect2);
            Assert.IsTrue(rect1.Equals(rect3));
            Assert.IsFalse(rect1.Equals("Object"));
        }
    }
}