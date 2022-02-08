using ManiaMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ManiaMapTests
{
    [TestClass]
    public class TestConfigurationSpace
    {
        private static RoomTemplate GetRoomTemplate1(int id)
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

        [TestMethod]
        public void TestFindConfigurations()
        {
            var from = GetRoomTemplate1(1);
            var to = GetRoomTemplate1(2);
            var space = new ConfigurationSpace(from, to);

            var expected = new (int, int, Door, Door)[]
            {
                (-3, 0, from.Cells[0, 1].TopDoor, to.Cells[2, 1].BottomDoor),
                (0, -3, from.Cells[1, 0].LeftDoor, to.Cells[1, 2].RightDoor),
                (0, 3, from.Cells[1, 2].RightDoor, to.Cells[1, 0].LeftDoor),
                (3, 0, from.Cells[2, 1].BottomDoor, to.Cells[0, 1].TopDoor),
            };

            var result = space.Configurations.Select(x => (x.X, x.Y, x.FromDoor, x.ToDoor)).ToArray();
            CollectionAssert.AreEqual(expected, result);
        }
    }
}