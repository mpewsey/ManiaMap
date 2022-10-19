using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Exceptions;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutNode
    {
        [TestMethod]
        public void TestToString()
        {
            var node = new LayoutNode(1).SetName("LayoutNodeName");
            var expected = "LayoutNode(Id = 1, Name = LayoutNodeName)";
            Assert.AreEqual(expected, node.ToString());
        }

        [TestMethod]
        public void TestSetColor()
        {
            var node = new LayoutNode(1).SetColor(new Color32(255, 0, 0, 255));
            Assert.AreEqual(new Color32(255, 0, 0, 255), node.Color);
        }

        [TestMethod]
        public void TestSetZ()
        {
            var node = new LayoutNode(1).SetZ(10);
            Assert.AreEqual(10, node.Z);
        }

        [TestMethod]
        public void TestSetTemplateGroup()
        {
            var node = new LayoutNode(1).SetTemplateGroup("Default");
            Assert.AreEqual("Default", node.TemplateGroup);
        }

        [TestMethod]
        public void TestCopy()
        {
            var node = new LayoutNode(1);
            var copy = node.Copy();

            foreach (var property in node.GetType().GetProperties())
            {
                Assert.AreEqual(property.GetValue(node), property.GetValue(copy));
            }
        }

        [TestMethod]
        public void TestNoTemplateGroupAssigned()
        {
            var node = new LayoutNode(1);
            Assert.ThrowsException<NoTemplateGroupAssignedException>(() => node.Validate());
        }

        [TestMethod]
        public void TestIsValid()
        {
            var node = new LayoutNode(1).SetTemplateGroup("Default");
            Assert.IsTrue(node.IsValid());
        }
    }
}