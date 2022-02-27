using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCell
    {
        [TestMethod]
        public void TestToString()
        {
            var cell = Cell.New;
            Assert.IsTrue(cell.ToString().StartsWith("Cell("));
        }

        [TestMethod]
        public void TestMatches()
        {
            var cell1 = Cell.New.SetDoors("NW", Door.TwoWay);
            var cell2 = cell1.Rotated180().Rotated180();
            Assert.IsTrue(cell1.Matches(cell2));
        }

        [TestMethod]
        public void TestDoesNotMatch()
        {
            var cell1 = Cell.New.SetDoors("NW", Door.TwoWay);
            var cell2 = cell1.Rotated180();
            Assert.IsFalse(cell1.Matches(cell2));
        }
    }
}