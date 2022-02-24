using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoor
    {
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
            };

            for (int i = directions.Count - 1; i >= 0; i--)
            {
                var (x, y, z) = directions[i];
                directions.Add((y, x, Door.ReverseEdgeDirection(z)));
            }

            var results = directions.Select(x => Door.GetEdgeDirection(x.Item1, x.Item2)).ToList();
            var expected = directions.Select(x => x.Item3).ToList();
            CollectionAssert.AreEqual(expected, results);
        }
    }
}