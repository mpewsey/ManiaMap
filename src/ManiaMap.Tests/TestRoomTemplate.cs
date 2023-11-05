using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoomTemplate
    {
        private static void AddCollectableSpots(RoomTemplate template, Array2D<int> ids, string group)
        {
            for (int i = 0; i < ids.Rows; i++)
            {
                for (int j = 0; j < ids.Columns; j++)
                {
                    template.AddCollectableSpot(ids[i, j], new Vector2DInt(i, j), group);
                }
            }
        }

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
            var template2 = template1.Rotated180(-1).Rotated180(-1);
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
        public void TestCollectableGroupNameNotValid()
        {
            var template = Samples.TemplateLibrary.Squares.Square3x3Template();
            Assert.ThrowsException<InvalidNameException>(() => template.AddCollectableSpot(100, Vector2DInt.Zero, ""));
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
            Assert.IsTrue(template.CellValuesAreEqual(template.Rotated90(-1)));
        }

        [TestMethod]
        public void TestSquareRotated180()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.Rotated180(-1)));
        }

        [TestMethod]
        public void TestSquareRotated270()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.Rotated270(-1)));
        }

        [TestMethod]
        public void TestSquareMirroredHorizontally()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.MirroredHorizontally(-1)));
        }

        [TestMethod]
        public void TestSquareMirroredVertically()
        {
            var template = Samples.TemplateLibrary.Squares.Square2x2Template();
            Assert.IsTrue(template.CellValuesAreEqual(template.MirroredVertically(-1)));
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

            var ids1 = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
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

            var ids2 = new int[,]
            {
                { 9, 5, 1 },
                { 10, 6, 2 },
                { 11, 7, 3 },
                { 12, 8, 4 },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1);
            var template2 = new RoomTemplate(2, "Test2", cells2);
            AddCollectableSpots(template1, ids1, "Default");
            AddCollectableSpots(template2, ids2, "Default");
            Assert.IsTrue(template2.IsEquivalentTo(template1.Rotated90(-1)));
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

            var ids1 = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            var c = Cell.New.SetDoors("NEB", Door.TwoWay);
            var d = Cell.New.SetDoors("SWT", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { o, x, o, c },
                { o, x, o, o },
                { d, o, o, x },
            };

            var ids2 = new int[,]
            {
                { 12, 11, 10, 9 },
                { 8, 7, 6, 5 },
                { 4, 3, 2, 1 },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1);
            var template2 = new RoomTemplate(2, "Test2", cells2);
            AddCollectableSpots(template1, ids1, "Default");
            AddCollectableSpots(template2, ids2, "Default");
            Assert.IsTrue(template2.IsEquivalentTo(template1.Rotated180(-1)));
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

            var ids1 = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
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

            var ids2 = new int[,]
            {
                { 4, 8, 12 },
                { 3, 7, 11 },
                { 2, 6, 10 },
                { 1, 5, 9 },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1);
            var template2 = new RoomTemplate(2, "Test2", cells2);
            AddCollectableSpots(template1, ids1, "Default");
            AddCollectableSpots(template2, ids2, "Default");
            Assert.IsTrue(template2.IsEquivalentTo(template1.Rotated270(-1)));
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

            var ids1 = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            var c = Cell.New.SetDoors("NWT", Door.TwoWay);
            var d = Cell.New.SetDoors("SEB", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, o, x },
                { o, x, o, o },
                { o, x, o, d },
            };

            var ids2 = new int[,]
            {
                { 4, 3, 2, 1 },
                { 8, 7, 6, 5 },
                { 12, 11, 10, 9 },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1);
            var template2 = new RoomTemplate(2, "Test2", cells2);
            AddCollectableSpots(template1, ids1, "Default");
            AddCollectableSpots(template2, ids2, "Default");
            Assert.IsTrue(template2.IsEquivalentTo(template1.MirroredHorizontally(-1)));
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

            var ids1 = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            var c = Cell.New.SetDoors("NWB", Door.TwoWay);
            var d = Cell.New.SetDoors("SET", Door.TwoWay);

            var cells2 = new Cell[,]
            {
                { c, o, x, o },
                { o, o, x, o },
                { x, o, o, d },
            };

            var ids2 = new int[,]
            {
                { 9, 10, 11, 12 },
                { 5, 6, 7, 8 },
                { 1, 2, 3, 4 },
            };

            var template1 = new RoomTemplate(1, "Test1", cells1);
            var template2 = new RoomTemplate(2, "Test2", cells2);
            AddCollectableSpots(template1, ids1, "Default");
            AddCollectableSpots(template2, ids2, "Default");
            Assert.IsTrue(template2.IsEquivalentTo(template1.MirroredVertically(-1)));
        }

        [TestMethod]
        public void TestCollectableSpotsAreValid()
        {
            var spots = new HashMap<int, CollectableSpot>
            {
                { 1, new CollectableSpot(Vector2DInt.Zero, "Default", 1) },
            };

            var x = Cell.Empty;
            var o = Cell.New.SetDoors("N", Door.TwoWay);

            var cells = new Cell[,]
            {
                { x, o, x },
                { x, x, x },
            };

            var template = new RoomTemplate(1, "Test", cells, spots);
            Assert.IsTrue(template.IsValid());
        }

        [TestMethod]
        public void TestValidateCollectableSpotPositions()
        {
            var spots = new HashMap<int, CollectableSpot>
            {
                { 1, new CollectableSpot(new Vector2DInt(-1, -1), "Default", 1) },
            };

            var x = Cell.Empty;
            var o = Cell.New.SetDoors("N", Door.TwoWay);

            var cells = new Cell[,]
            {
                { x, o, x },
                { x, x, x },
            };

            var template = new RoomTemplate(1, "Test", cells, spots);
            Assert.ThrowsException<IndexOutOfRangeException>(() => template.Validate());
        }

        [TestMethod]
        public void TestValidateCollectableSpotGroupNames()
        {
            var spots = new HashMap<int, CollectableSpot>
            {
                { 1, new CollectableSpot(Vector2DInt.Zero, "", 1) },
            };

            var x = Cell.Empty;
            var o = Cell.New.SetDoors("N", Door.TwoWay);

            var cells = new Cell[,]
            {
                { x, o, x },
                { x, x, x },
            };

            var template = new RoomTemplate(1, "Test", cells, spots);
            Assert.ThrowsException<InvalidNameException>(() => template.Validate());
        }

        [TestMethod]
        public void TestCollectableSpotsAreNotValid()
        {
            var spots = new HashMap<int, CollectableSpot>
            {
                { 1, new CollectableSpot(Vector2DInt.Zero, "", 1) },
            };

            var x = Cell.Empty;
            var o = Cell.New.SetDoors("N", Door.TwoWay);

            var cells = new Cell[,]
            {
                { x, o, x },
                { x, x, x },
            };

            var template = new RoomTemplate(1, "Test", cells, spots);
            Assert.IsFalse(template.IsValid());
        }

        [TestMethod]
        public void TestCollectableSpotValuesAreNotEqual()
        {
            var spots1 = new HashMap<int, CollectableSpot>
            {
                { 1, new CollectableSpot(Vector2DInt.Zero, "", 1) },
            };

            var spots2 = new HashMap<int, CollectableSpot>
            {
                { 2, new CollectableSpot(Vector2DInt.Zero, "", 1) },
            };

            var template1 = new RoomTemplate(1, "Test", new Array2D<Cell>(2, 3));
            var template2 = new RoomTemplate(1, "Test", new Array2D<Cell>(2, 3), spots1);
            var template3 = new RoomTemplate(1, "Test", new Array2D<Cell>(2, 3), spots2);

            Assert.IsFalse(template1.ValuesAreEqual(template2));
            Assert.IsFalse(template2.ValuesAreEqual(template3));
        }
    }
}