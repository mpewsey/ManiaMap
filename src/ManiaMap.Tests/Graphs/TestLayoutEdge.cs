using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Exceptions;

namespace MPewsey.ManiaMap.Graphs.Tests
{
    [TestClass]
    public class TestLayoutEdge
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "LayoutEdge.xml";
            var edge = new LayoutEdge(1, 2);
            XmlSerialization.SaveXml(path, edge);
            var copy = XmlSerialization.LoadXml<LayoutEdge>(path);
            Assert.AreEqual(edge.Name, copy.Name);
            Assert.AreEqual(edge.FromNode, copy.FromNode);
            Assert.AreEqual(edge.ToNode, copy.ToNode);
            Assert.AreEqual(edge.Direction, copy.Direction);
            Assert.AreEqual(edge.DoorCode, copy.DoorCode);
            Assert.AreEqual(edge.Z, copy.Z);
            Assert.AreEqual(edge.RoomChance, copy.RoomChance);
            Assert.AreEqual(edge.RequireRoom, copy.RequireRoom);
            Assert.AreEqual(edge.Color, copy.Color);
            Assert.AreEqual(edge.TemplateGroup, copy.TemplateGroup);
            Assert.AreEqual(edge.RoomId, copy.RoomId);
        }

        [TestMethod]
        public void TestToString()
        {
            var edge = new LayoutEdge(1, 2);
            Assert.IsTrue(edge.ToString().StartsWith("LayoutEdge("));
        }

        [TestMethod]
        public void TestToSymbolString()
        {
            var edge = new LayoutEdge(1, 2);
            edge.Direction = EdgeDirection.Both;
            Assert.AreEqual("(1 <=> 2)", edge.ToSymbolString());
            edge.Direction = EdgeDirection.ForwardFlexible;
            Assert.AreEqual("(1 => 2)", edge.ToSymbolString());
            edge.Direction = EdgeDirection.ForwardFixed;
            Assert.AreEqual("(1 -> 2)", edge.ToSymbolString());
            edge.Direction = EdgeDirection.ReverseFlexible;
            Assert.AreEqual("(1 <= 2)", edge.ToSymbolString());
            edge.Direction = EdgeDirection.ReverseFixed;
            Assert.AreEqual("(1 <- 2)", edge.ToSymbolString());
        }

        [TestMethod]
        public void TestSetDoorCode()
        {
            var edge = new LayoutEdge(1, 2).SetDoorCode(DoorCode.A);
            Assert.AreEqual(DoorCode.A, edge.DoorCode);
        }

        [TestMethod]
        public void TestSetDirection()
        {
            var edge = new LayoutEdge(1, 2).SetDirection(EdgeDirection.ForwardFlexible);
            Assert.AreEqual(EdgeDirection.ForwardFlexible, edge.Direction);
        }

        [TestMethod]
        public void TestSetTemplateGroup()
        {
            var edge = new LayoutEdge(1, 2).SetTemplateGroup("Test");
            Assert.AreEqual("Test", edge.TemplateGroup);
        }

        [TestMethod]
        public void TestSetColor()
        {
            var edge = new LayoutEdge(1, 2).SetColor(new Color4(255, 0, 0, 255));
            Assert.AreEqual(new Color4(255, 0, 0, 255), edge.Color);
        }

        [TestMethod]
        public void TestSetRoomChance()
        {
            var edge = new LayoutEdge(1, 2).SetRoomChance(0.5f);
            Assert.AreEqual(0.5f, edge.RoomChance);
        }

        [TestMethod]
        public void TestSetZ()
        {
            var edge = new LayoutEdge(1, 2).SetZ(1);
            Assert.AreEqual(1, edge.Z);
        }

        [TestMethod]
        public void TestSetName()
        {
            var edge = new LayoutEdge(1, 2).SetName("Edge1");
            Assert.AreEqual("Edge1", edge.Name);
        }

        [TestMethod]
        public void TestCopy()
        {
            var edge = new LayoutEdge(1, 2);
            var copy = edge.Copy();

            foreach (var property in edge.GetType().GetProperties())
            {
                Assert.AreEqual(property.GetValue(edge), property.GetValue(copy));
            }
        }

        [TestMethod]
        public void TestNoTemplateGroupAssigned()
        {
            var edge = new LayoutEdge(1, 2).SetRoomChance(1).SetTemplateGroup(null);
            Assert.ThrowsException<NoTemplateGroupAssignedException>(() => edge.Validate());
        }

        [TestMethod]
        public void TestIsValid()
        {
            var edge = new LayoutEdge(1, 2).SetRoomChance(1).SetTemplateGroup("Default");
            Assert.IsTrue(edge.IsValid());
        }
    }
}