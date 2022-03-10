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
            var result = space.Configurations.Select(x => (x.Position, x.FromDoor.Position, x.ToDoor.Position)).ToArray();

            var expected = new (Vector2DInt, Vector2DInt, Vector2DInt)[]
            {
                (new Vector2DInt(-3, 0), new Vector2DInt(0, 1), new Vector2DInt(2, 1)),
                (new Vector2DInt(0, -3), new Vector2DInt(1, 0), new Vector2DInt(1, 2)),
                (new Vector2DInt(0, 0), new Vector2DInt(1, 1), new Vector2DInt(1, 1)),
                (new Vector2DInt(0, 0), new Vector2DInt(1, 1), new Vector2DInt(1, 1)),
                (new Vector2DInt(0, 3), new Vector2DInt(1, 2), new Vector2DInt(1, 0)),
                (new Vector2DInt(3, 0), new Vector2DInt(2, 1), new Vector2DInt(0, 1)),
            };

            Console.WriteLine("Expected:");
            Console.WriteLine(string.Join("\n", expected));

            Console.WriteLine("\nResults:");
            Console.WriteLine(string.Join("\n", space.Configurations));

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToString()
        {
            var from = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var to = Samples.TemplateLibrary.Miscellaneous.PlusTemplate();
            var space = new ConfigurationSpace(from, to);
            Assert.IsTrue(space.ToString().StartsWith("ConfigurationSpace("));
        }
    }
}