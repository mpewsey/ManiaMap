using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestConfigurationSpace
    {
        private static RoomTemplate GetRoomTemplate1(int id)
        {
            var o = new Cell();
            var l = new Cell { WestDoor = new(DoorType.TwoWay) };
            var t = new Cell { NorthDoor = new(DoorType.TwoWay) };
            var r = new Cell { EastDoor = new(DoorType.TwoWay) };
            var b = new Cell { SouthDoor = new(DoorType.TwoWay) };
            var a = new Cell { TopDoor = new(DoorType.TwoWay), BottomDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { a, t, o },
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
                (-3, 0, from.Cells[0, 1].NorthDoor, to.Cells[2, 1].SouthDoor),
                (0, -3, from.Cells[1, 0].WestDoor, to.Cells[1, 2].EastDoor),
                (0, 0, from.Cells[0, 0].TopDoor, to.Cells[0, 0].BottomDoor),
                (0, 0, from.Cells[0, 0].BottomDoor, to.Cells[0, 0].TopDoor),
                (0, 3, from.Cells[1, 2].EastDoor, to.Cells[1, 0].WestDoor),
                (3, 0, from.Cells[2, 1].SouthDoor, to.Cells[0, 1].NorthDoor),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));
            Console.WriteLine("\nResults:");
            Console.WriteLine(string.Join("\n", space.Configurations));

            var result = space.Configurations.Select(x => (x.X, x.Y, x.FromDoor, x.ToDoor)).ToArray();
            CollectionAssert.AreEqual(expected, result);
        }
    }
}