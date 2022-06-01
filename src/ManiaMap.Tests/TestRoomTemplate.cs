using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;
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
            var doors = from.AlignedDoors(to, new Vector2DInt(0, 0)).ToList();
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
        public void TestCellValuesEqual()
        {
            var template1 = Samples.TemplateLibrary.Miscellaneous.SquareTemplate();
            var template2 = template1.Rotated180().Rotated180();
            Assert.IsTrue(template1.CellValuesAreEqual(template2));
        }

        [TestMethod]
        public void TestIsFullyConnected()
        {
            var template = Samples.TemplateLibrary.Angles.Angle3x4();
            Assert.IsTrue(template.IsFullyConnected());
        }

        [TestMethod]
        public void TestEmptyIsFullyConnected()
        {
            var template = new RoomTemplate(1, "Test", new Array2D<Cell>());
            Assert.IsTrue(template.IsFullyConnected());
        }

        [TestMethod]
        public void TestIsNotFullyConnected()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { x, o, o, x },
                { o, o, x, o },
                { x, o, o, x },
            };

            var template = new RoomTemplate(1, "Test", cells);
            Assert.IsFalse(template.IsFullyConnected());
        }

        [TestMethod]
        public void TestCellsNotFullyConnected()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { x, o, o, x },
                { o, o, x, o },
                { x, o, o, x },
            };

            var template = new RoomTemplate(1, "Test", cells);
            Assert.ThrowsException<CellsNotFullyConnectedException>(() => template.Validate());
        }

        [TestMethod]
        public void TestNoDoorsExist()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { x, o, x },
                { o, o, o },
                { x, o, x },
            };

            var template = new RoomTemplate(1, "Test", cells);
            Assert.ThrowsException<NoDoorsExistException>(() => template.Validate());
        }

        [TestMethod]
        public void TestAnyDoorExists()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            Assert.IsTrue(template.AnyDoorExists());
        }

        [TestMethod]
        public void TestAnyDoorDoesNotExist()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { x, o, x },
                { o, o, o },
                { x, o, x },
            };

            var template = new RoomTemplate(1, "Test", cells);
            Assert.IsFalse(template.AnyDoorExists());
        }

        [TestMethod]
        public void TestDuplicateCollectableId()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            template.Cells[0, 0].AddCollectableSpot(1, "Default");
            template.Cells[0, 1].AddCollectableSpot(1, "Default");
            Assert.ThrowsException<DuplicateIdException>(() => template.Validate());
        }

        [TestMethod]
        public void TestCollectableSpotIdsAreUnique()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            template.Cells[0, 0].AddCollectableSpot(1, "Default");
            Assert.IsTrue(template.CollectableSpotIdsAreUnique());
        }

        [TestMethod]
        public void TestCollectableSpotIdsAreNotUnique()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            template.Cells[0, 0].AddCollectableSpot(1, "Default");
            template.Cells[0, 1].AddCollectableSpot(1, "Default");
            Assert.IsFalse(template.CollectableSpotIdsAreUnique());
        }

        [TestMethod]
        public void TestIsValid()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            Assert.IsTrue(template.IsValid());
        }

        [TestMethod]
        public void TestSaveAndLoadPrettyXml()
        {
            var path = "Angle3x4PrettyPrint.xml";
            var template = Samples.TemplateLibrary.Angles.Angle3x4();
            Serialization.SavePrettyXml(path, template);
            var copy = Serialization.LoadXml<RoomTemplate>(path);
            Assert.AreEqual(template.Id, copy.Id);
        }
    }
}