using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestShaft
    {
        [TestMethod]
        public void TestTemplateDoesNotIntersect()
        {
            var ring = Samples.TemplateLibrary.RingTemplate();
            var shaft = new Shaft(8, 8, 10, 10, -10, 10);
            Assert.IsFalse(shaft.Intersects(ring, 7, 9, 0));
        }

        [TestMethod]
        public void TestTemplateIntersects()
        {
            var square = Samples.TemplateLibrary.SquareTemplate();
            var shaft = new Shaft(8, 8, 10, 10, -10, 10);
            Assert.IsTrue(shaft.Intersects(square, 8, 10, 0));
        }
    }
}