using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestShaft
    {
        private static RoomTemplate GetRingTemplate()
        {
            Cell x = null;
            var o = new Cell();

            var cells = new Cell[,]
            {
                { o, o, o },
                { o, x, o },
                { o, o, o },
            };

            return new(1, cells);
        }

        private static RoomTemplate GetSquareTemplate()
        {
            var o = new Cell();

            var cells = new Cell[,]
            {
                { o, o, o },
                { o, o, o },
                { o, o, o },
            };

            return new(2, cells);
        }

        [TestMethod]
        public void TestTemplateDoesNotIntersect()
        {
            var ring = GetRingTemplate();
            var shaft = new Shaft(8, 8, 10, 10, -10, 10);
            Assert.IsFalse(shaft.Intersects(ring, 7, 9, 0));
        }

        [TestMethod]
        public void TestTemplateIntersects()
        {
            var square = GetSquareTemplate();
            var shaft = new Shaft(8, 8, 10, 10, -10, 10);
            Assert.IsTrue(shaft.Intersects(square, 8, 10, 0));
        }
    }
}