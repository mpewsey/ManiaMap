using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestBox
    {
        [TestMethod]
        public void TestTemplateDoesNotIntersect()
        {
            var ring = Samples.TemplateLibrary.Miscellaneous.RingTemplate();
            var shaft = new Box(8, 8, 10, 10, -10, 10);
            Assert.IsFalse(shaft.Intersects(ring, 7, 9, 0));
        }

        [TestMethod]
        public void TestTemplateIntersects()
        {
            var square = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var shaft = new Box(8, 8, 10, 10, -10, 10);
            Assert.IsTrue(shaft.Intersects(square, 8, 10, 0));
        }

        [TestMethod]
        public void TestRangeIntersects()
        {
            var shaft = new Box(8, 8, 10, 10, -10, 10);
            Assert.IsTrue(shaft.Intersects(8, 8, 10, 10, 0, 0));
        }

        [TestMethod]
        public void TestRangeDoesNotIntersect()
        {
            var shaft = new Box(8, 8, 10, 10, -10, 10);
            Assert.IsFalse(shaft.Intersects(0, 0, 0, 0, 0, 0));
        }

        [TestMethod]
        public void TestToString()
        {
            var result = new Box(1, 2, 3, 4, 5, 6).ToString();
            var expected = "Box(XMin = 1, XMax = 2, YMin = 3, YMax = 4, ZMin = 5, ZMax = 6)";
            Assert.AreEqual(expected, result);
        }
    }
}