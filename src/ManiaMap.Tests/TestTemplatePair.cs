using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Collections;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestTemplatePair
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var a = new RoomTemplate(1, "First", new Array2D<Cell>());
            var b = new RoomTemplate(2, "Second", new Array2D<Cell>());
            var x = new TemplatePair(a, b);
            var y = new TemplatePair(a, b);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var a = new RoomTemplate(1, "First", new Array2D<Cell>());
            var b = new RoomTemplate(2, "Second", new Array2D<Cell>());
            var x = new TemplatePair(a, b);
            var y = new TemplatePair(b, a);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var a = new RoomTemplate(1, "First", new Array2D<Cell>());
            var b = new RoomTemplate(2, "Second", new Array2D<Cell>());
            var x = new TemplatePair(a, b);
            var y = new TemplatePair(a, b);
            Assert.IsTrue(x.Equals((object)y));
        }

        [TestMethod]
        public void TestToString()
        {
            var a = new RoomTemplate(1, "First", new Array2D<Cell>());
            var b = new RoomTemplate(2, "Second", new Array2D<Cell>());
            var x = new TemplatePair(a, b);
            Assert.IsTrue(x.ToString().StartsWith("TemplatePair("));
        }
    }
}