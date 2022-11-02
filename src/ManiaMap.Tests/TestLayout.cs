using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;
using System.IO;
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
        public void TestSaveAndLoadManiaMapLayoutXml()
        {
            var path = "ManiaMapLayout.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            XmlSerialization.SaveXml(path, layout);
            var copy = XmlSerialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed.Seed, copy.Seed.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadBigLaoutXml()
        {
            var path = "Layout.xml";
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            var layout = (Layout)results.Outputs["Layout"];
            XmlSerialization.SaveXml(path, layout);
            var copy = XmlSerialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed.Seed, copy.Seed.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadXmlBytes()
        {
            var path = "ManiaMapLayoutBytes.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            XmlSerialization.SaveXml(path, layout);
            var bytes = File.ReadAllBytes(path);
            var copy = XmlSerialization.LoadXml<Layout>(bytes);
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
        public void TestContainsDoor()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var connection = layout.DoorConnections.Values.First();
            Assert.IsTrue(connection.ContainsDoor(connection.FromRoom, connection.FromDoor.Position, connection.FromDoor.Direction));
            Assert.IsTrue(connection.ContainsDoor(connection.ToRoom, connection.ToDoor.Position, connection.ToDoor.Direction));
            Assert.IsFalse(connection.ContainsDoor(connection.FromRoom, connection.ToDoor.Position, connection.ToDoor.Direction));
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