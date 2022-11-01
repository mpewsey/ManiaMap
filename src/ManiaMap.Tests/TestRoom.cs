using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var layout = (Layout)results.Outputs["Layout"];
            var room = layout.Rooms.Values.First();
            Serialization.SaveXml(path, room);
            var copy = Serialization.LoadXml<Room>(path);
            Assert.AreEqual(room.Id, copy.Id);
            Assert.AreEqual(room.Name, copy.Name);
            Assert.AreEqual(room.Position, copy.Position);
            Assert.AreEqual(room.Seed, copy.Seed);
            Assert.AreEqual(room.Color, copy.Color);
            Assert.AreEqual(room.Template.Id, copy.Template.Id);
            Assert.AreEqual(room.Collectables.Count, copy.Collectables.Count);

            foreach (var pair in room.Collectables)
            {
                Assert.AreEqual(pair.Value, copy.Collectables[pair.Key]);
            }
        }
    }
}