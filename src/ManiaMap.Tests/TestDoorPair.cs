using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoorPair
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var a = new DoorPosition(0, 0, DoorDirection.North, Door.TwoWay);
            var b = new DoorPosition(1, 2, DoorDirection.South, Door.TwoWay);
            var x = new DoorPair(a, b);
            var y = new DoorPair(a, b);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var a = new DoorPosition(0, 0, DoorDirection.North, Door.TwoWay);
            var b = new DoorPosition(1, 2, DoorDirection.South, Door.TwoWay);
            var x = new DoorPair(a, b);
            var y = new DoorPair(b, a);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var a = new DoorPosition(0, 0, DoorDirection.North, Door.TwoWay);
            var b = new DoorPosition(1, 2, DoorDirection.South, Door.TwoWay);
            var x = new DoorPair(a, b);
            var y = new DoorPair(a, b);
            Assert.IsTrue(x.Equals((object)y));
        }

        [TestMethod]
        public void TestToString()
        {
            var a = new DoorPosition(0, 0, DoorDirection.North, Door.TwoWay);
            var b = new DoorPosition(1, 2, DoorDirection.South, Door.TwoWay);
            var x = new DoorPair(a, b);
            Assert.IsTrue(x.ToString().StartsWith("DoorPair("));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var a = new DoorPosition(0, 0, DoorDirection.North, Door.TwoWay);
            var b = new DoorPosition(1, 2, DoorDirection.South, Door.TwoWay);
            var x = new DoorPair(a, b);
            var y = new DoorPair(a, b);
            Assert.AreEqual(x.GetHashCode(), y.GetHashCode());
        }
    }
}