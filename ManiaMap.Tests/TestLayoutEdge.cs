using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
        public void TestAddTemplateGroup()
        {
            var edge = new LayoutEdge(1, 2).AddTemplateGroups("Default");
            var expected = new List<string>() { "Default" };
            CollectionAssert.AreEquivalent(expected, edge.TemplateGroups);
        }

        [TestMethod]
        public void TestAddTemplateGroups()
        {
            var edge = new LayoutEdge(1, 2).AddTemplateGroups("Default", "Default", "MoreTemplates");
            var expected = new List<string>() { "Default", "MoreTemplates" };
            CollectionAssert.AreEquivalent(expected, edge.TemplateGroups);
        }

        [TestMethod]
        public void TestSetColor()
        {
            var edge = new LayoutEdge(1, 2).SetColor(Color.Red);
            Assert.AreEqual(Color.Red, edge.Color);
        }
    }
}