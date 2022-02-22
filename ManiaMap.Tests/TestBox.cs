﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}