using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoomState
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "RoomState.xml";
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            var room = layout.Rooms.Values.First();
            var state = new RoomState(room);
            XmlSerialization.SaveXml(path, state);
            var copy = XmlSerialization.LoadXml<RoomState>(path);
            Assert.AreEqual(state.Id, copy.Id);
            Assert.AreEqual(state.VisibleCells.Rows, copy.VisibleCells.Rows);
            Assert.AreEqual(state.VisibleCells.Columns, copy.VisibleCells.Columns);
            CollectionAssert.AreEqual(state.VisibleCells.Array, copy.VisibleCells.Array);
            CollectionAssert.AreEquivalent(state.AcquiredCollectables.ToList(), copy.AcquiredCollectables.ToList());
            CollectionAssert.AreEquivalent(state.Flags.ToList(), copy.Flags.ToList());
        }
    }
}