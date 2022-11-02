using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestBox
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Box.xml";
            var box = new Box(new Vector3DInt(1, 2, 3), new Vector3DInt(4, 5, 6));
            XmlSerialization.SaveXml(path, box);
            var copy = XmlSerialization.LoadXml<Box>(path);
            Assert.AreEqual(box.Min, copy.Min);
            Assert.AreEqual(box.Max, copy.Max);
        }

        [TestMethod]
        public void TestTemplateDoesNotIntersect()
        {
            var ring = Samples.TemplateLibrary.Miscellaneous.RingTemplate();
            var shaft = new Box(new Vector3DInt(8, 10, -10), new Vector3DInt(8, 10, 10));
            Assert.IsFalse(shaft.Intersects(ring, new Vector3DInt(7, 9, 0)));
        }

        [TestMethod]
        public void TestTemplateIntersects()
        {
            var square = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var shaft = new Box(new Vector3DInt(8, 10, -10), new Vector3DInt(8, 10, 10));
            Assert.IsTrue(shaft.Intersects(square, new Vector3DInt(8, 10, 0)));
        }

        [TestMethod]
        public void TestRangeIntersects()
        {
            var shaft = new Box(new Vector3DInt(8, 10, -10), new Vector3DInt(8, 10, 10));
            Assert.IsTrue(shaft.Intersects(new Vector3DInt(8, 10, 0), new Vector3DInt(8, 10, 0)));
        }

        [TestMethod]
        public void TestRangeDoesNotIntersect()
        {
            var shaft = new Box(new Vector3DInt(8, 10, -10), new Vector3DInt(8, 10, 10));
            Assert.IsFalse(shaft.Intersects(Vector3DInt.Zero, Vector3DInt.Zero));
        }

        [TestMethod]
        public void TestToString()
        {
            var result = new Box(new Vector3DInt(1, 3, 5), new Vector3DInt(2, 4, 6)).ToString();
            var expected = "Box(Min = Vector3DInt(1, 3, 5), Max = Vector3DInt(2, 4, 6))";
            Assert.AreEqual(expected, result);
        }
    }
}