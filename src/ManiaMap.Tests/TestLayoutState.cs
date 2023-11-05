using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutState
    {
        [TestMethod]
        public void TestSaveAndLoadManiaMapXml()
        {
            var path = "ManiaMapLayoutState.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            var state = new LayoutState(layout);
            XmlSerialization.SaveXml(path, state);
            var copy = XmlSerialization.LoadXml<LayoutState>(path);
            Assert.AreEqual(state.Id, copy.Id);
            Assert.AreEqual(state.RoomStates.Count, copy.RoomStates.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadBigLayoutXml()
        {
            var path = "BigLayoutState.xml";
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            var state = new LayoutState(layout);
            XmlSerialization.SaveXml(path, state);
            var copy = XmlSerialization.LoadXml<LayoutState>(path);
            Assert.AreEqual(state.Id, copy.Id);
            Assert.AreEqual(state.RoomStates.Count, copy.RoomStates.Count);
        }
    }
}