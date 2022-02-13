using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutMap
    {
        private static RoomTemplate GetMTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, x, x, x, o },
                { o, o, o, o, o },
                { o, x, o, x, o },
                { o, x, o, x, o },
                { b, x, x, x, a },
            };

            return new(1, cells);
        }

        private static RoomTemplate GetATemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, o, o },
                { x, x, o },
                { o, o, o },
                { o, x, o },
                { b, o, a },
            };

            return new(2, cells);
        }

        private static RoomTemplate GetNTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { x, x, x },
                { o, o, o },
                { o, x, o },
                { o, x, o },
                { b, x, a },
            };

            return new(3, cells);
        }

        private static RoomTemplate GetITemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { RightDoor = new(DoorType.TwoWay) };
            var b = new Cell { LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, o, o },
                { x, o, x },
                { x, o, x },
                { x, o, x },
                { b, o, a },
            };

            return new(3, cells);
        }

        private static RoomTemplate GetPTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { LeftDoor = new(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, o, o },
                { o, x, o },
                { o, o, o },
                { o, x, x },
                { a, x, x },
            };

            return new(4, cells);
        }

        private static Layout GetManiaMapLayout()
        {
            var m = GetMTemplate();
            var a = GetATemplate();
            var n = GetNTemplate();
            var i = GetITemplate();
            var p = GetPTemplate();

            var layout = new Layout(0);
            var m1 = new Room(0, 0, 0, m);
            var a1 = new Room(1, 0, 5, a);
            var n1 = new Room(2, 0, 8, n);
            var i1 = new Room(3, 0, 11, i);
            var a2 = new Room(4, 0, 14, a);
            var m2 = new Room(5, 0, 17, m);
            var a3 = new Room(6, 0, 22, a);
            var p1 = new Room(7, 0, 25, p);

            layout.Rooms[m1.Id] = m1;
            layout.Rooms[a1.Id] = a1;
            layout.Rooms[n1.Id] = n1;
            layout.Rooms[i1.Id] = i1;
            layout.Rooms[a2.Id] = a2;
            layout.Rooms[m2.Id] = m2;
            layout.Rooms[a3.Id] = a3;
            layout.Rooms[p1.Id] = p1;

            layout.DoorConnections.AddRange(new DoorConnection[]
            {
                new(m1, a1, m1.Template.Cells[4, 4].RightDoor, a1.Template.Cells[4, 0].LeftDoor),
                new(a1, n1, a1.Template.Cells[4, 2].RightDoor, n1.Template.Cells[4, 0].LeftDoor),
                new(n1, i1, n1.Template.Cells[4, 2].RightDoor, i1.Template.Cells[4, 0].LeftDoor),
                new(i1, a2, i1.Template.Cells[4, 2].RightDoor, a1.Template.Cells[4, 0].LeftDoor),
                new(a2, m2, a2.Template.Cells[4, 2].RightDoor, m2.Template.Cells[4, 0].LeftDoor),
                new(m2, a3, m2.Template.Cells[4, 4].RightDoor, a3.Template.Cells[4, 0].LeftDoor),
                new(a3, p1, a3.Template.Cells[4, 2].RightDoor, p1.Template.Cells[4, 0].LeftDoor),
            });

            return layout;
        }

        [TestMethod]
        public void TestSavePng()
        {
            var layout = GetManiaMapLayout();

            Console.WriteLine("Rooms:");
            var rooms = layout.Rooms.Values.ToList();
            rooms.ForEach(x => Console.WriteLine(x));

            Console.WriteLine("\nDoors:");
            layout.DoorConnections.ForEach(x => Console.WriteLine(x));

            var map = new LayoutMap(layout, padding: 4);
            map.SavePng("ManiaMap.png");
        }
    }
}
