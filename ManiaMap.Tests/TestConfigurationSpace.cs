using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestConfigurationSpace
    {
        [TestMethod]
        public void TestFindConfigurations()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var space = new ConfigurationSpace(from, to);
            var result = space.Configurations.Select(x => (x.X, x.Y, x.FromDoor, x.ToDoor)).ToArray();

            var expected = new (int, int, Door, Door)[]
            {
                (-3, 0, from.Cells[0, 1].NorthDoor, to.Cells[2, 1].SouthDoor),
                (0, -3, from.Cells[1, 0].WestDoor, to.Cells[1, 2].EastDoor),
                (0, 0, from.Cells[1, 1].TopDoor, to.Cells[1, 1].BottomDoor),
                (0, 0, from.Cells[1, 1].BottomDoor, to.Cells[1, 1].TopDoor),
                (0, 3, from.Cells[1, 2].EastDoor, to.Cells[1, 0].WestDoor),
                (3, 0, from.Cells[2, 1].SouthDoor, to.Cells[0, 1].NorthDoor),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResults:");
            Console.WriteLine(string.Join("\n", space.Configurations));

            CollectionAssert.AreEqual(expected, result);
        }
    }
}