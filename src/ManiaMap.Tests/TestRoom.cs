using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;
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
    }
}