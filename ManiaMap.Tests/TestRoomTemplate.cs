using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ManiaMap.Tests
{
    [TestClass]
    public class TestRoomTemplate
    {
        private static RoomTemplate GetBoxRoom(int id)
        {
            var o = new Cell();
            var l = new Cell { LeftDoor = new(DoorType.TwoWay) };
            var t = new Cell { TopDoor = new(DoorType.TwoWay) };
            var r = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { BottomDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, t, o },
                { l, o, r },
                { o, b, o },
            };

            return new(id, cells);
        }

        private static RoomTemplate GetPlusRoom(int id)
        {
            Cell x = null;
            var o = new Cell();
            var l = new Cell { LeftDoor = new(DoorType.TwoWay) };
            var t = new Cell { TopDoor = new(DoorType.TwoWay) };
            var r = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { BottomDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { x, t, x },
                { l, o, r },
                { x, b, x },
            };

            return new(id, cells);
        }

        [TestMethod]
        public void TestDoesNotIntersect()
        {
            var from = GetPlusRoom(1);
            var to = GetPlusRoom(2);
            Assert.IsFalse(from.Intersects(to, 2, 2));
        }

        [TestMethod]
        public void TestIntersects()
        {
            var from = GetPlusRoom(1);
            var to = GetPlusRoom(2);
            Assert.IsTrue(from.Intersects(to, 0, 0));
        }

        [TestMethod]
        public void TestNoDoorsAlign()
        {
            var from = GetBoxRoom(1);
            var to = GetBoxRoom(2);
            var doors = from.AlignedDoors(to, 0, 0);
            var expected = new List<DoorPair>();
            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestRightDoorAligns()
        {
            var from = GetBoxRoom(1);
            var to = GetBoxRoom(2);
            var doors = from.AlignedDoors(to, 0, 3);

            var expected = new List<DoorPair>
            {
                new(from.Cells[1, 2].RightDoor, to.Cells[1, 0].LeftDoor),
            };

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestLeftDoorAligns()
        {
            var from = GetBoxRoom(1);
            var to = GetBoxRoom(2);
            var doors = from.AlignedDoors(to, 0, -3);

            var expected = new List<DoorPair>
            {
                new(from.Cells[1, 0].LeftDoor, to.Cells[1, 2].RightDoor),
            };

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestTopDoorAligns()
        {
            var from = GetBoxRoom(1);
            var to = GetBoxRoom(2);
            var doors = from.AlignedDoors(to, -3, 0);

            var expected = new List<DoorPair>
            {
                new(from.Cells[0, 1].TopDoor, to.Cells[2, 1].BottomDoor),
            };

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestBottomDoorAligns()
        {
            var from = GetBoxRoom(1);
            var to = GetBoxRoom(2);
            var doors = from.AlignedDoors(to, 3, 0);

            var expected = new List<DoorPair>
            {
                new(from.Cells[2, 1].BottomDoor, to.Cells[0, 1].TopDoor),
            };

            CollectionAssert.AreEqual(expected, doors);
        }
    }
}