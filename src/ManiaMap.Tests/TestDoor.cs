using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Graphs;
using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoor
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Door.xml";
            var door = Door.OneWayEntrance.SetCode(100);
            XmlSerialization.SaveXml(path, door);
            var copy = XmlSerialization.LoadXml<Door>(path);
            Assert.IsTrue(door.ValuesAreEqual(copy));
        }

        [TestMethod]
        public void TestSetCode()
        {
            var door = Door.TwoWay.SetCode(100);
            Assert.AreEqual(100, door.Code);
        }

        [TestMethod]
        public void TestStaticInitializers()
        {
            Assert.AreEqual(DoorType.TwoWay, Door.TwoWay.Type);
            Assert.AreEqual(DoorType.OneWayExit, Door.OneWayExit.Type);
            Assert.AreEqual(DoorType.OneWayEntrance, Door.OneWayEntrance.Type);
            Assert.AreEqual(DoorType.TwoWayExit, Door.TwoWayExit.Type);
            Assert.AreEqual(DoorType.TwoWayEntrance, Door.TwoWayEntrance.Type);
        }

        [TestMethod]
        public void TestDoorTypeAligns()
        {
            var types = new List<(DoorType, DoorType, bool)>()
            {
                (DoorType.TwoWay, DoorType.TwoWay, true),
                (DoorType.TwoWay, DoorType.TwoWayExit, true),
                (DoorType.TwoWay, DoorType.OneWayExit, true),
                (DoorType.TwoWay, DoorType.OneWayEntrance, true),
                (DoorType.TwoWay, DoorType.TwoWayEntrance, true),
                (DoorType.OneWayExit, DoorType.OneWayEntrance, true),
                (DoorType.OneWayEntrance, DoorType.OneWayExit, true),
            };

            for (int i = types.Count - 1; i >= 0; i--)
            {
                var (x, y, z) = types[i];
                types.Add((y, x, z));
            }

            var expected = types.Select(x => x.Item3).ToList();
            var results = types.Select(x => Door.DoorTypesAlign(x.Item1, x.Item2)).ToList();
            CollectionAssert.AreEquivalent(expected, results);
        }

        [TestMethod]
        public void TestGetEdgeDirection()
        {
            var directions = new List<(DoorType, DoorType, EdgeDirection)>()
            {
                (DoorType.TwoWay, DoorType.TwoWay, EdgeDirection.Both),
                (DoorType.TwoWay, DoorType.TwoWayEntrance, EdgeDirection.ForwardFlexible),
                (DoorType.TwoWay, DoorType.TwoWayExit, EdgeDirection.ReverseFlexible),
                (DoorType.TwoWay, DoorType.OneWayEntrance, EdgeDirection.ForwardFixed),
                (DoorType.TwoWay, DoorType.OneWayExit, EdgeDirection.ReverseFixed),
                (DoorType.TwoWayExit, DoorType.TwoWayEntrance, EdgeDirection.ForwardFlexible),
                (DoorType.TwoWayExit, DoorType.OneWayEntrance, EdgeDirection.ForwardFixed),
                (DoorType.OneWayExit, DoorType.OneWayEntrance, EdgeDirection.ForwardFixed),
                (DoorType.TwoWayEntrance, DoorType.OneWayExit, EdgeDirection.ReverseFixed),
            };

            for (int i = directions.Count - 1; i >= 0; i--)
            {
                var (x, y, z) = directions[i];
                directions.Add((y, x, LayoutEdge.ReverseEdgeDirection(z)));
            }

            var results = directions.Select(x => Door.GetEdgeDirection(x.Item1, x.Item2)).ToList();
            var expected = directions.Select(x => x.Item3).ToList();
            CollectionAssert.AreEqual(expected, results);
        }

        [TestMethod]
        public void TestGetEdgeDirectionExceptions()
        {
            var directions = new List<(DoorType, DoorType)>()
            {
                (DoorType.TwoWayExit, DoorType.TwoWayExit),
                (DoorType.TwoWayExit, DoorType.OneWayExit),
                (DoorType.TwoWayEntrance, DoorType.TwoWayEntrance),
                (DoorType.TwoWayEntrance, DoorType.OneWayEntrance),
                (DoorType.OneWayExit, DoorType.TwoWayExit),
                (DoorType.OneWayExit, DoorType.OneWayExit),
                (DoorType.OneWayEntrance, DoorType.TwoWayEntrance),
                (DoorType.OneWayEntrance, DoorType.OneWayEntrance),
            };

            foreach (var (x, y) in directions)
            {
                Assert.ThrowsException<ArgumentException>(() => Door.GetEdgeDirection(x, y));
            }
        }

        [TestMethod]
        public void TestCopy()
        {
            var door = new Door(DoorType.OneWayExit, 10);
            var copy = door.Copy();

            foreach (var property in door.GetType().GetProperties())
            {
                if (!property.GetGetMethod().IsStatic)
                {
                    Assert.AreEqual(property.GetValue(door), property.GetValue(copy));
                }
            }
        }

        [TestMethod]
        public void TestValuesAreEqual()
        {
            var door = Door.TwoWay;
            Assert.IsTrue(Door.ValuesAreEqual(door, door));
            Assert.IsTrue(Door.ValuesAreEqual(door, door.Copy()));
            Assert.IsFalse(Door.ValuesAreEqual(door, null));
        }
    }
}