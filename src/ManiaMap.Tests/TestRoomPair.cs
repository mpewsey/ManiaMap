using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoomPair
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            var value1 = new RoomPair(x, y);
            var value2 = new RoomPair(x, y);
            Assert.IsTrue(value1 == value2);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            var value1 = new RoomPair(x, y);
            var value2 = new RoomPair(y, x);
            Assert.IsTrue(value1 != value2);
        }

        [TestMethod]
        public void TestComparison()
        {
            var x1 = new RoomPair(new Uid(1), new Uid(2));
            var y1 = new RoomPair(new Uid(1), new Uid(2));
            Assert.AreEqual(0, x1.CompareTo(y1));

            var x2 = new RoomPair(new Uid(1), new Uid(4));
            var y2 = new RoomPair(new Uid(2), new Uid(3));
            Assert.AreEqual(-1, x2.CompareTo(y2));
        }

        [TestMethod]
        public void TestToString()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            var value = new RoomPair(x, y);
            Assert.IsTrue(value.ToString().StartsWith("RoomPair("));
        }
    }
}