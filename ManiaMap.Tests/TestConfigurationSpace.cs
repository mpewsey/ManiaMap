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
            var result = space.Configurations.Select(x => (x.X, x.Y, x.FromDoor.X, x.FromDoor.Y, x.ToDoor.X, x.ToDoor.Y)).ToArray();

            var expected = new (int, int, int, int, int, int)[]
            {
                (-3, 0, 0, 1, 2, 1),
                (0, -3, 1, 0, 1, 2),
                (0, 0, 1, 1, 1, 1),
                (0, 0, 1, 1, 1, 1),
                (0, 3, 1, 2, 1, 0),
                (3, 0, 2, 1, 0, 1),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResults:");
            Console.WriteLine(string.Join("\n", space.Configurations));

            CollectionAssert.AreEqual(expected, result);
        }
    }
}