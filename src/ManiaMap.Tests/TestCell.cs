using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCell
    {
        [TestMethod]
        public void TestToString()
        {
            var cell = Cell.New;
            Assert.IsTrue(cell.ToString().StartsWith("Cell("));
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
            Assert.ThrowsException<Exception>(() => Cell.New.SetDoors("x", Door.TwoWay));
        }

        [TestMethod]
        public void TestAddCollectableGroup()
        {
            var cell = Cell.New.AddCollectableGroup(0, "Group1").AddCollectableGroup(1, "Group2");
            var expected = new List<Collectable> { new Collectable(0, "Group1"), new Collectable(1, "Group2") };
            CollectionAssert.AreEqual(expected, cell.CollectableGroups);
        }
    }
}