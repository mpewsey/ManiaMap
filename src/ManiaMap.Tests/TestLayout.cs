using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Random;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayout
    {
        [TestMethod]
        public void TestToString()
        {
            var layout = new Layout(1, "TestLayout", 12345);
            Assert.IsTrue(layout.ToString().StartsWith("Layout("));
        }

        [TestMethod]
        public void TestSaveAndLoadManiaMapLayoutXml()
        {
            var path = "ManiaMapLayout.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            XmlSerialization.SaveXml(path, layout);
            var copy = XmlSerialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed, copy.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadBigLayoutXml()
        {
            var path = "Layout.xml";
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            XmlSerialization.SaveXml(path, layout);
            var copy = XmlSerialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed, copy.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
            Assert.AreEqual(layout.Templates.Count, copy.Templates.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadBigLayoutJson()
        {
            var path = "Layout.json";
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            JsonSerialization.SaveJson(path, layout);
            var copy = JsonSerialization.LoadJson<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed, copy.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
            Assert.AreEqual(layout.Templates.Count, copy.Templates.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadEncryptedJson()
        {
            var path = "LayoutJson.sav";
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");

            var key = new byte[32];
            var random = new Random(12345);
            random.NextBytes(key);

            JsonSerialization.SaveEncryptedJson(path, layout, key);
            var copy = JsonSerialization.LoadEncryptedJson<Layout>(path, key);
            Assert.AreEqual(layout.Id, copy.Id);
            Assert.AreEqual(layout.Name, copy.Name);
            Assert.AreEqual(layout.Seed, copy.Seed);
            Assert.AreEqual(layout.Rooms.Count, copy.Rooms.Count);
            Assert.AreEqual(layout.DoorConnections.Count, copy.DoorConnections.Count);
            Assert.AreEqual(layout.Templates.Count, copy.Templates.Count);
        }

        [TestMethod]
        public void TestSaveAndLoadXmlBytes()
        {
            var path = "ManiaMapLayoutBytes.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            XmlSerialization.SaveXml(path, layout);
            var bytes = File.ReadAllBytes(path);
            var copy = XmlSerialization.LoadXml<Layout>(bytes);
            Assert.AreEqual(layout.Seed, copy.Seed);
        }

        [TestMethod]
        public void TestGetRoomConnections()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            Assert.IsTrue(results.Success);
            var layout = results.GetOutput<Layout>("Layout");
            var connections = layout.GetRoomConnections();
            Assert.AreEqual(layout.Rooms.Count, connections.Count);
        }

        [TestMethod]
        public void TestGetDoorConnection()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            var connection = layout.DoorConnections.Values.First();
            Assert.AreEqual(connection, layout.GetDoorConnection(connection.FromRoom, connection.ToRoom));
            Assert.AreEqual(connection, layout.GetDoorConnection(connection.ToRoom, connection.FromRoom));
        }

        [TestMethod]
        public void TestContainsDoor()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            var connection = layout.DoorConnections.Values.First();
            Assert.IsTrue(connection.ContainsDoor(connection.FromRoom, connection.FromDoor.Position, connection.FromDoor.Direction));
            Assert.IsTrue(connection.ContainsDoor(connection.ToRoom, connection.ToDoor.Position, connection.ToDoor.Direction));
            Assert.IsFalse(connection.ContainsDoor(connection.FromRoom, connection.ToDoor.Position, connection.ToDoor.Direction));
        }

        [TestMethod]
        public void TestRemoveDoorConnection()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout(Console.WriteLine);
            var connection = layout.DoorConnections.Values.First();
            Assert.IsTrue(layout.RemoveDoorConnection(connection.FromRoom, connection.ToRoom));
        }

        [TestMethod]
        public void TestGenerateConstrainedLayout()
        {
            var seed = new RandomSeed(12345);
            var graph = Samples.GraphLibrary.BigGraph();
            var groups = Samples.BigLayoutSample.BigLayoutTemplateGroups();
            var collectables = new CollectableGroups();

            foreach (var entry in groups.GetGroup("Rooms"))
            {
                entry.MinQuantity = 2;
            }

            var dict = new Dictionary<string, object>
            {
                { "LayoutId", 1 },
                { "LayoutGraph", graph },
                { "TemplateGroups", groups },
                { "CollectableGroups", collectables },
                { "RandomSeed", seed },
            };

            var pipeline = PipelineBuilder.CreateDefaultPipeline();
            var results = pipeline.Run(dict);
            Assert.IsTrue(results.Success);
        }

        [TestMethod]
        public void TestVisibleCellCount()
        {
            var seed = new RandomSeed(12345);
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            var layout = results.GetOutput<Layout>("Layout");
            var layoutState = new LayoutState(layout);

            foreach (var roomState in layoutState.RoomStates.Values)
            {
                var visibleCells = roomState.VisibleCells;

                for (int i = 0; i < visibleCells.Rows; i++)
                {
                    for (int j = 0; j < visibleCells.Columns; j++)
                    {
                        if (seed.ChanceSatisfied(0.5f))
                            visibleCells[i, j] = true;
                    }
                }
            }

            var counts = layout.VisibleCellCount(layoutState);
            Assert.IsTrue(counts.X > 0);
            Assert.IsTrue(counts.Y > 0);
            var progress = counts.X / (float)counts.Y;
            Assert.IsTrue(progress >= 0.4f);
            Assert.IsTrue(progress <= 0.6f);
        }

        [TestMethod]
        public void TestVisibleCellProgress()
        {
            var seed = new RandomSeed(12345);
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            var layout = results.GetOutput<Layout>("Layout");
            var layoutState = new LayoutState(layout);

            foreach (var roomState in layoutState.RoomStates.Values)
            {
                var visibleCells = roomState.VisibleCells;

                for (int i = 0; i < visibleCells.Rows; i++)
                {
                    for (int j = 0; j < visibleCells.Columns; j++)
                    {
                        if (seed.ChanceSatisfied(0.5f))
                            visibleCells[i, j] = true;
                    }
                }
            }

            var progress = layout.VisibleCellProgress(layoutState);
            Assert.IsTrue(progress >= 0.4f);
            Assert.IsTrue(progress <= 0.6f);
        }

        [TestMethod]
        public void TestFindRoomWithTag()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            var layout = results.GetOutput<Layout>("Layout");
            var room = layout.FindRoomWithTag("Origin");
            Assert.IsNotNull(room);
            Assert.AreEqual(new Uid(1), room.Id);
        }

        [TestMethod]
        public void TestFindRoomsWithTag()
        {
            var results = Samples.BigLayoutSample.Generate(12345, Console.WriteLine);
            var layout = results.GetOutput<Layout>("Layout");
            var rooms = layout.FindRoomsWithTag("Origin");
            Assert.IsNotNull(rooms);
            Assert.AreEqual(1, rooms.Count);
            Assert.AreEqual(new Uid(1), rooms[0].Id);
        }
    }
}