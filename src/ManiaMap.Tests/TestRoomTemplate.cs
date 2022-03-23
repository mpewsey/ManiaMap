using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoomTemplate
    {
        [TestMethod]
        public void TestDoesNotIntersect()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            Assert.IsFalse(from.Intersects(to, new Vector2DInt(2, 2)));
        }

        [TestMethod]
        public void TestIntersects()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            Assert.IsTrue(from.Intersects(to, new Vector2DInt(0, 0)));
        }

        [TestMethod]
        public void TestNoDoorsAlign()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var doors = from.AlignedDoors(to, new Vector2DInt(0, 0));
            var expected = new List<DoorPair>();
            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestRightDoorAligns()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var doors = from.AlignedDoors(to, new Vector2DInt(0, 3)).Select(x => (x.FromDoor.Position, x.ToDoor.Position)).ToArray();

            var expected = new (Vector2DInt, Vector2DInt)[]
            {
                (new Vector2DInt(1, 2), new Vector2DInt(1, 0)),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResult:");
            Console.WriteLine(string.Join("\n", doors));

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestLeftDoorAligns()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var doors = from.AlignedDoors(to, new Vector2DInt(0, -3)).Select(x => (x.FromDoor.Position, x.ToDoor.Position)).ToArray();

            var expected = new (Vector2DInt, Vector2DInt)[]
            {
                (new Vector2DInt(1, 0), new Vector2DInt(1, 2)),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResult:");
            Console.WriteLine(string.Join("\n", doors));

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestTopDoorAligns()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var doors = from.AlignedDoors(to, new Vector2DInt(-3, 0)).Select(x => (x.FromDoor.Position, x.ToDoor.Position)).ToArray();

            var expected = new (Vector2DInt, Vector2DInt)[]
            {
                (new Vector2DInt(0, 1), new Vector2DInt(2, 1)),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResult:");
            Console.WriteLine(string.Join("\n", doors));

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestBottomDoorAligns()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var doors = from.AlignedDoors(to, new Vector2DInt(3, 0)).Select(x => (x.FromDoor.Position, x.ToDoor.Position)).ToArray();

            var expected = new (Vector2DInt, Vector2DInt)[]
            {
                (new Vector2DInt(2, 1), new Vector2DInt(0, 1)),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResult:");
            Console.WriteLine(string.Join("\n", doors));

            CollectionAssert.AreEqual(expected, doors);
        }

        [TestMethod]
        public void TestCellsMatch()
        {
            var template1 = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var template2 = template1.Rotated180().Rotated180();
            Assert.IsTrue(template1.CellsMatch(template2));
        }

        [TestMethod]
        public void TestFindDistances1()
        {
            var template = Samples.TemplateLibrary.Miscellaneous.RingTemplate();
            var distances = template.FindCellDistances(new Vector2DInt(0, 1));

            var expected = new int[,]
            {
                { 1,  0, 1 },
                { 2, -1, 2 },
                { 3,  4, 3 },
            };

            var array = new Array2D<int>(expected);
            CollectionAssert.AreEqual(array.Array, distances.Array);
        }

        [TestMethod]
        public void TestFindDistances2()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { o, o, o, o },
                { o, x, o, o },
                { o, o, x, o },
                { o, o, x, o },
                { o, o, o, o },
            };

            var template = new RoomTemplate(1, "FindDistances", cells);
            var distances = template.FindCellDistances(Vector2DInt.Zero);

            var expected = new int[,]
            {
                { 0,  1,  2, 3 },
                { 1, -1,  3, 4 },
                { 2,  3, -1, 5 },
                { 3,  4, -1, 6 },
                { 4,  5,  6, 7 },
            };

            var array = new Array2D<int>(expected);
            CollectionAssert.AreEqual(array.Array, distances.Array);
        }
    }
}