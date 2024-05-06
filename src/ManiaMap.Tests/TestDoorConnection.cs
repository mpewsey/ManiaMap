using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoorConnection
    {
        [TestMethod]
        public void TestEdgeDirection()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");

            foreach (var connection in layout.DoorConnections.Values)
            {
                Assert.AreEqual(EdgeDirection.Both, connection.EdgeDirection);
            }
        }

        [TestMethod]
        public void TestGetDoorPosition()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");

            foreach (var connection in layout.DoorConnections.Values)
            {
                var position = connection.GetDoorPosition(connection.FromRoom);
                Assert.AreEqual(connection.FromDoor, position);
                position = connection.GetDoorPosition(connection.ToRoom);
                Assert.AreEqual(connection.ToDoor, position);
                position = connection.GetDoorPosition(new Uid(-1, -1, -1));
                Assert.IsNull(position);
            }
        }

        [TestMethod]
        public void TestGetConnectingRoom()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");

            foreach (var connection in layout.DoorConnections.Values)
            {
                var toRoom = connection.ToRoom;
                var fromRoom = connection.FromRoom;
                Assert.AreEqual(connection.GetConnectingRoom(toRoom), fromRoom);
                Assert.AreEqual(connection.GetConnectingRoom(fromRoom), toRoom);
                Assert.AreEqual(connection.GetConnectingRoom(new Uid(int.MaxValue)), new Uid(-1, -1, -1));
            }
        }
    }
}
