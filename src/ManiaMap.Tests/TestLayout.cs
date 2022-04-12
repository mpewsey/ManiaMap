using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayout
    {
        [TestMethod]
        public void TestToString()
        {
            var layout = new Layout(1, "TestLayout", new RandomSeed(12345));
            Assert.IsTrue(layout.ToString().StartsWith("Layout("));
        }

        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "ManiaMapLayout.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            Serialization.SaveXml(path, layout);
            var copy = Serialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Seed.Seed, copy.Seed.Seed);
        }

        [TestMethod]
        public void TestGetDoorConnection()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var connection = layout.DoorConnections.Values.First();
            Assert.AreEqual(connection, layout.GetDoorConnection(connection.FromRoom, connection.ToRoom));
            Assert.AreEqual(connection, layout.GetDoorConnection(connection.ToRoom, connection.FromRoom));
        }

        [TestMethod]
        public void TestRemoveDoorConnection()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var connection = layout.DoorConnections.Values.First();
            Assert.IsTrue(layout.RemoveDoorConnection(connection.FromRoom, connection.ToRoom));
        }
    }
}