using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Exceptions;

namespace MPewsey.ManiaMap.Graphs.Tests
{
    [TestClass]
    public class TestLayoutNode
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "LayoutNode.xml";
            var node = new LayoutNode(100);
            XmlSerialization.SaveXml(path, node);
            var copy = XmlSerialization.LoadXml<LayoutNode>(path);
            Assert.AreEqual(node.Id, copy.Id);
            Assert.AreEqual(node.Name, copy.Name);
            Assert.AreEqual(node.Z, copy.Z);
            Assert.AreEqual(node.TemplateGroup, copy.TemplateGroup);
            Assert.AreEqual(node.Color, copy.Color);
            Assert.AreEqual(node.RoomId, copy.RoomId);
        }

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
            var node = new LayoutNode(1).SetColor(new Color4(255, 0, 0, 255));
            Assert.AreEqual(new Color4(255, 0, 0, 255), node.Color);
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
            var node = new LayoutNode(1).SetTemplateGroup("Test");
            Assert.AreEqual("Test", node.TemplateGroup);
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
            var node = new LayoutNode(1).SetTemplateGroup(null);
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