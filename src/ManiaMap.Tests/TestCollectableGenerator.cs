using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectableGenerator
    {
        [TestMethod]
        public void TestGenerateLayoutForBigLayout()
        {
            var graph = Samples.GraphLibrary.BigGraph();
            var templateGroups = new TemplateGroups();
            var collectableGroups = new CollectableGroups();

            var square2x2 = Samples.TemplateLibrary.Squares.Square2x2Template();
            var square3x3 = Samples.TemplateLibrary.Squares.Square3x3Template();
            var rect2x3 = Samples.TemplateLibrary.Rectangles.Rectangle2x3Template();
            var rect2x4 = Samples.TemplateLibrary.Rectangles.Rectangle2x4Template();

            var square1x1 = Samples.TemplateLibrary.Squares.Square1x1Template();
            var rect1x2 = Samples.TemplateLibrary.Rectangles.Rectangle1x2Template();
            var rect1x3 = Samples.TemplateLibrary.Rectangles.Rectangle1x3Template();
            var rect1x4 = Samples.TemplateLibrary.Rectangles.Rectangle1x4Template();
            var angle3x4 = Samples.TemplateLibrary.Angles.Angle3x4();

            var templates = new List<RoomTemplate>
            {
                square2x2,
                square3x3,
                rect2x3,
                rect2x4,
                square1x1,
                rect1x2,
                rect1x3,
                rect1x4,
                angle3x4,
            };

            foreach (var template in templates)
            {
                foreach (var cell in template.Cells.Array)
                {
                    cell?.SetCollectableGroup("Default");
                }
            }

            templateGroups.Add("Rooms",
                square2x2.UniqueVariations(),
                square3x3.UniqueVariations(),
                rect2x3.UniqueVariations(),
                rect2x4.UniqueVariations());

            templateGroups.Add("Paths",
                square1x1.UniqueVariations(),
                rect1x2.UniqueVariations(),
                rect1x3.UniqueVariations(),
                rect1x4.UniqueVariations(),
                angle3x4.UniqueVariations());

            var expected = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            collectableGroups.Add("Default", expected);
            var generator = new LayoutGenerator();
            var random = new RandomSeed(12345);
            var layout = generator.Generate(1, graph, templateGroups, random);
            Assert.IsNotNull(layout);

            var collectableGenerator = new CollectableGenerator();
            collectableGenerator.Generate(layout, collectableGroups, random);
            var result = new List<int>();

            foreach (var room in layout.Rooms.Values)
            {
                result.AddRange(room.Collectables.Values);
            }

            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}