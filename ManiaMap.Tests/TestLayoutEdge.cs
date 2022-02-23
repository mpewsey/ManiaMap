using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}