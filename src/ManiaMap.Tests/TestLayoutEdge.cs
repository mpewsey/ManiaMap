using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutEdge
    {
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
            var edge = new LayoutEdge(1, 2).SetDoorCode(1);
            Assert.AreEqual(1, edge.DoorCode);
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
            var edge = new LayoutEdge(1, 2).SetTemplateGroup("Default");
            Assert.AreEqual("Default", edge.TemplateGroup);
        }

        [TestMethod]
        public void TestSetColor()
        {
            var edge = new LayoutEdge(1, 2).SetColor(Color.Red);
            Assert.AreEqual(Color.Red, edge.Color);
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
    }
}