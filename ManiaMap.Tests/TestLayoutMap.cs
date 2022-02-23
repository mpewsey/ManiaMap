using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Drawing.Tests
{
    [TestClass]
    public class TestLayoutMap
    {
        [TestMethod]
        public void TestSaveImage()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();

            Assert.IsNotNull(layout);

            Console.WriteLine("Rooms:");
            Console.WriteLine(string.Join("\n", layout.Rooms.Values));

            Console.WriteLine("\nDoors:");
            Console.WriteLine(string.Join("\n", layout.DoorConnections));

            var map = new LayoutMap(layout, padding: new Padding(4));
            map.SaveImage("ManiaMap.png");
        }

        [TestMethod]
        public void TestToString()
        {
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var map = new LayoutMap(layout);
            Assert.IsTrue(map.ToString().StartsWith("LayoutMap("));
        }
    }
}
