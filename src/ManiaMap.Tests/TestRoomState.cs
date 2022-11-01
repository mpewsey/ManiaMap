using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var layout = (Layout)results.Outputs["Layout"];
            var room = layout.Rooms.Values.First();
            var state = new RoomState(room);
            Serialization.SaveXml(path, state);
            var copy = Serialization.LoadXml<RoomState>(path);
            Assert.AreEqual(state.Id, copy.Id);
            Assert.AreEqual(state.VisibleCells.Rows, copy.VisibleCells.Rows);
            Assert.AreEqual(state.VisibleCells.Columns, copy.VisibleCells.Columns);
            CollectionAssert.AreEqual(state.VisibleCells.Array, copy.VisibleCells.Array);
            CollectionAssert.AreEquivalent(state.AcquiredCollectables.ToList(), copy.AcquiredCollectables.ToList());
            CollectionAssert.AreEquivalent(state.Flags.ToList(), copy.Flags.ToList());
        }
    }
}