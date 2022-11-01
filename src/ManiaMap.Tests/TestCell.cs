using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCell
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Cell.xml";
            var cell = Cell.New.SetDoors("NWSETB", Door.TwoWay).AddFeature("SavePoint").AddFeature("Boss").AddCollectableSpot(1, "Test");
            Serialization.SaveXml(path, cell);
            var copy = Serialization.LoadXml<Cell>(path);
            Assert.IsTrue(cell.ValuesAreEqual(copy));
        }

        [TestMethod]
        public void TestToString()
        {
            var cell = Cell.New.SetDoors("N", Door.TwoWay).AddFeature("SavePoint");
            var str = cell.ToString();
            Console.WriteLine(str);
            Assert.IsTrue(str.StartsWith("Cell("));
        }

        [TestMethod]
        public void TestValuesEqual()
        {
            var cell1 = Cell.New.SetDoors("NW", Door.TwoWay);
            var cell2 = cell1.Rotated180().Rotated180();
            Assert.IsTrue(cell1.ValuesAreEqual(cell2));
        }

        [TestMethod]
        public void TestValuesAreNotEqual()
        {
            var cell1 = Cell.New.SetDoors("NW", Door.TwoWay);
            var cell2 = cell1.Rotated180();
            Assert.IsFalse(cell1.ValuesAreEqual(cell2));
        }

        [TestMethod]
        public void TestSetDoors()
        {
            var cell = Cell.New.SetDoors("NWSETB", Door.TwoWay);
            Assert.IsNotNull(cell.TopDoor);
            Assert.IsNotNull(cell.BottomDoor);
            Assert.IsNotNull(cell.NorthDoor);
            Assert.IsNotNull(cell.SouthDoor);
            Assert.IsNotNull(cell.WestDoor);
            Assert.IsNotNull(cell.EastDoor);
            Assert.ThrowsException<UnhandledCaseException>(() => Cell.New.SetDoors("x", Door.TwoWay));
        }

        [TestMethod]
        public void TestAddCollectableSpot()
        {
            var cell = Cell.New.AddCollectableSpot(0, "Group1").AddCollectableSpot(1, "Group2");
            var expected = new List<Collectable> { new Collectable(0, "Group1"), new Collectable(1, "Group2") };
            CollectionAssert.AreEqual(expected, cell.CollectableSpots.Select(x => new Collectable(x.Key, x.Value)).ToList());
        }

        [TestMethod]
        public void TestInvalidCollectableGroupName()
        {
            Assert.ThrowsException<InvalidNameException>(() => Cell.New.AddCollectableSpot(0, ""));
        }

        [TestMethod]
        public void TestDuplicateLocationId()
        {
            var cell = Cell.New.AddCollectableSpot(1, "Default");
            Assert.ThrowsException<DuplicateIdException>(() => cell.AddCollectableSpot(1, "Default"));
        }

        [TestMethod]
        public void TestValuesAreEqual()
        {
            var cell1 = Cell.New.AddCollectableSpot(1, "Group1");
            var cell2 = Cell.New.AddCollectableSpot(1, "Group2");
            Assert.IsFalse(Cell.ValuesAreEqual(cell1, cell2));

            var cell3 = Cell.New.SetDoors("N", Door.TwoWay);
            var cell4 = Cell.New.SetDoors("N", Door.OneWayEntrance);
            Assert.IsFalse(Cell.ValuesAreEqual(cell3, cell4));

            var cell5 = Cell.New.AddCollectableSpot(1, "Group1").SetDoors("NWSE", Door.TwoWay);
            var cell6 = Cell.New.AddCollectableSpot(1, "Group1").SetDoors("NWSE", Door.TwoWay);
            Assert.IsTrue(Cell.ValuesAreEqual(cell5, cell6));

            var cell7 = Cell.New.AddCollectableSpot(1, "Group1");
            var cell8 = Cell.New;
            Assert.IsFalse(Cell.ValuesAreEqual(cell7, cell8));

            var cell9 = Cell.New.SetDoors("N", Door.TwoWay);
            var cell10 = Cell.New;
            Assert.IsFalse(Cell.ValuesAreEqual(cell9, cell10));

            var cell11 = Cell.New.SetDoors("N", Door.TwoWay).AddFeature("SavePoint");
            var cell12 = Cell.New.SetDoors("N", Door.TwoWay);
            Assert.IsFalse(Cell.ValuesAreEqual(cell11, cell12));

            var cell13 = Cell.New.SetDoors("N", Door.TwoWay).AddFeature("SavePoint");
            var cell14 = Cell.New.SetDoors("N", Door.TwoWay).AddFeature("Boss");
            Assert.IsFalse(Cell.ValuesAreEqual(cell13, cell14));
        }
    }
}