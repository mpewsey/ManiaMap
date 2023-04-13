using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Random;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Graphs;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRoom
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Room.xml";
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            var room = layout.Rooms.Values.First();
            XmlSerialization.SaveXml(path, room);
            var copy = XmlSerialization.LoadXml<Room>(path);
            Assert.AreEqual(room.Id, copy.Id);
            Assert.AreEqual(room.Name, copy.Name);
            Assert.AreEqual(room.Position, copy.Position);
            Assert.AreEqual(room.Seed, copy.Seed);
            Assert.AreEqual(room.Color, copy.Color);
            Assert.AreEqual(room.TemplateId, copy.TemplateId);
            Assert.AreEqual(room.Collectables.Count, copy.Collectables.Count);

            foreach (var pair in room.Collectables)
            {
                Assert.AreEqual(pair.Value, copy.Collectables[pair.Key]);
            }
        }

        [TestMethod]
        public void TestVisibleCellCount()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { o, o, x },
                { o, x, x },
                { o, o, o },
            };

            var node = new LayoutNode(1).SetName("Test");
            var seed = new RandomSeed(12345);
            var template = new RoomTemplate(1, "Test", cells);
            var room = new Room(node, Vector2DInt.Zero, template, seed);
            var state = new RoomState(room);
            var visibleCells = state.VisibleCells;

            visibleCells[0, 0] = true;
            visibleCells[0, 2] = true; // Cell does not exist here so should not contribute to count.
            visibleCells[1, 0] = true;
            visibleCells[2, 0] = true;

            var cellCount = 6;
            var visibleCount = 3;
            var counts = room.VisibleCellCount(state);

            Assert.AreEqual(visibleCount, counts.X);
            Assert.AreEqual(cellCount, counts.Y);
        }

        [TestMethod]
        public void TestVisibleCellProgress()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { o, o, x },
                { o, x, x },
                { o, o, o },
            };

            var node = new LayoutNode(1).SetName("Test");
            var seed = new RandomSeed(12345);
            var template = new RoomTemplate(1, "Test", cells);
            var room = new Room(node, Vector2DInt.Zero, template, seed);
            var state = new RoomState(room);
            var visibleCells = state.VisibleCells;

            visibleCells[0, 0] = true;
            visibleCells[0, 2] = true; // Cell does not exist here so should not contribute to count.
            visibleCells[1, 0] = true;
            visibleCells[2, 0] = true;

            Assert.AreEqual(0.5f, room.VisibleCellProgress(state));
        }
    }
}