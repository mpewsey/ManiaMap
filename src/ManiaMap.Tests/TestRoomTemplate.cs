using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoomTemplate
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "RoomTemplate.xml";
            var template = new RoomTemplate(1, "Test", new Array2D<Cell>(2, 3));
            XmlSerialization.SaveXml(path, template);
            var copy = XmlSerialization.LoadXml<RoomTemplate>(path);
            Assert.AreEqual(template.Id, copy.Id);
            Assert.AreEqual(template.Name, copy.Name);
            Assert.IsTrue(template.CellValuesAreEqual(copy));
        }

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
            o.Doors.Add(DoorDirection.North, null);

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
        public void TestCollectableGroupNameNotValid()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            template.Cells[0, 0].CollectableSpots[1] = "";
            Assert.ThrowsException<InvalidNameException>(() => template.Validate());
        }

        [TestMethod]
        public void TestIsValid()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            Assert.IsTrue(template.IsValid());
        }

        [TestMethod]
        public void TestValuesAreEqual()
        {
            var template1 = Samples.TemplateLibrary.Angles.Angle3x4();
            var template2 = Samples.TemplateLibrary.Angles.Angle3x4();
            Assert.IsTrue(RoomTemplate.ValuesAreEqual(template1, template1));
            Assert.IsTrue(RoomTemplate.ValuesAreEqual(template1, template2));
            Assert.IsFalse(RoomTemplate.ValuesAreEqual(template1, null));
        }

        [TestMethod]
        public void TestSquareRotated90()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.Rotated90()));
        }

        [TestMethod]
        public void TestSquareRotated180()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.Rotated180()));
        }

        [TestMethod]
        public void TestSquareRotated270()
        {
            var template1 = Samples.TemplateLibrary.Squares.Square2x2Template();
            var template2 = template1.Rotated270();
            Assert.IsTrue(template1.CellValuesAreEqual(template2));
        }

        [TestMethod]
        public void TestSquareMirroredHorizontally()
        {
            var template1 = Samples.TemplateLibrary.Squares.Square2x2Template();
            var template2 = template1.MirroredHorizontally();
            Assert.IsTrue(template1.CellValuesAreEqual(template2));
        }

        [TestMethod]
        public void TestSquareMirroredVertically()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.MirroredVertically()));
        }

        [TestMethod]
        public void TestUniqueVariations()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.AreEqual(1, template.UniqueVariations().Count);
        }

        [TestMethod]
        public void TestRotated90()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("NET", Door.TwoWay);
            var b = Cell.New.SetDoors("SWB", Door.TwoWay);

            var cells1 = new Cell[,]
            {
                { x, o, o, a },
                { o, o, x, o },
                { b, o, x, o },
            };

            var c = Cell.New.SetDoors("NWB", Door.TwoWay);
            var d = Cell.New.SetDoors("SET", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, x },
                { o, o, o },
                { x, x, o },
                { o, o, d },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1).Copy();
            var template2 = new RoomTemplate(2, "Test2", cells2).Copy();
            Assert.IsTrue(template2.CellValuesAreEqual(template1.Rotated90()));
        }

        [TestMethod]
        public void TestRotated180()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("NET", Door.TwoWay);
            var b = Cell.New.SetDoors("SWB", Door.TwoWay);

            var cells1 = new Cell[,]
            {
                { x, o, o, a },
                { o, o, x, o },
                { b, o, x, o },
            };

            var c = Cell.New.SetDoors("NEB", Door.TwoWay);
            var d = Cell.New.SetDoors("SWT", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { o, x, o, c },
                { o, x, o, o },
                { d, o, o, x },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1).Copy();
            var template2 = new RoomTemplate(2, "Test2", cells2).Copy();
            Assert.IsTrue(template2.CellValuesAreEqual(template1.Rotated180()));
        }

        [TestMethod]
        public void TestRotated270()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("NET", Door.TwoWay);
            var b = Cell.New.SetDoors("SWB", Door.TwoWay);

            var cells1 = new Cell[,]
            {
                { x, o, o, a },
                { o, o, x, o },
                { b, o, x, o },
            };

            var c = Cell.New.SetDoors("NWT", Door.TwoWay);
            var d = Cell.New.SetDoors("SEB", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, o },
                { o, x, x },
                { o, o, o },
                { x, o, d },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1).Copy();
            var template2 = new RoomTemplate(2, "Test2", cells2).Copy();
            Assert.IsTrue(template2.CellValuesAreEqual(template1.Rotated270()));
        }

        [TestMethod]
        public void TestMirroredHorizontally()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("NET", Door.TwoWay);
            var b = Cell.New.SetDoors("SWB", Door.TwoWay);

            var cells1 = new Cell[,]
            {
                { x, o, o, a },
                { o, o, x, o },
                { b, o, x, o },
            };

            var c = Cell.New.SetDoors("NWT", Door.TwoWay);
            var d = Cell.New.SetDoors("SEB", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, o, x },
                { o, x, o, o },
                { o, x, o, d },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1).Copy();
            var template2 = new RoomTemplate(2, "Test2", cells2).Copy();
            Assert.IsTrue(template2.CellValuesAreEqual(template1.MirroredHorizontally()));
        }

        [TestMethod]
        public void TestMirroredVertically()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("NET", Door.TwoWay);
            var b = Cell.New.SetDoors("SWB", Door.TwoWay);

            var cells1 = new Cell[,]
            {
                { x, o, o, a },
                { o, o, x, o },
                { b, o, x, o },
            };

            var c = Cell.New.SetDoors("NWB", Door.TwoWay);
            var d = Cell.New.SetDoors("SET", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, x, o },
                { o, o, x, o },
                { x, o, o, d },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1).Copy();
            var template2 = new RoomTemplate(2, "Test2", cells2).Copy();
            Assert.IsTrue(template2.CellValuesAreEqual(template1.MirroredVertically()));
        }
    }
}