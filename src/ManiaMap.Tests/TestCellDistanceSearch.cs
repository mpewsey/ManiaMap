using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCellDistanceSearch
    {
        [TestMethod]
        public void TestFindDistances1()
        {
            var template = Samples.TemplateLibrary.Miscellaneous.RingTemplate();
            var distances = template.FindCellDistances(new Vector2DInt(0, 1));

            var expected = new int[,]
            {
                { 1,  0, 1 },
                { 2, -1, 2 },
                { 3,  4, 3 },
            };

            var array = new Array2D<int>(expected);
            CollectionAssert.AreEqual(array.Array, distances.Array);
        }

        [TestMethod]
        public void TestFindDistances2()
        {
            var x = Cell.Empty;
            var o = Cell.New;

            var cells = new Cell[,]
            {
                { o, o, o, o },
                { o, x, o, o },
                { o, o, x, o },
                { o, o, x, o },
                { o, o, o, o },
            };

            var template = new RoomTemplate(1, "FindDistances", cells);
            var distances = template.FindCellDistances(Vector2DInt.Zero);

            var expected = new int[,]
            {
                { 0,  1,  2, 3 },
                { 1, -1,  3, 4 },
                { 2,  3, -1, 5 },
                { 3,  4, -1, 6 },
                { 4,  5,  6, 7 },
            };

            var array = new Array2D<int>(expected);
            CollectionAssert.AreEqual(array.Array, distances.Array);
        }
    }
}