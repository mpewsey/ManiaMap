using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Drawing.Tests
{
    [TestClass]
    public class TestPadding
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Padding(1, 2, 3, 4);
            var y = new Padding(1, 2, 3, 4);
            Assert.IsTrue(x == y);
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Padding(1, 2, 3, 4);
            var y = new Padding(1, 2, 3, 5);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestEquals()
        {
            var x = new Padding(1, 2, 3, 4);
            var y = new Padding(1, 2, 3, 4);
            Assert.IsTrue(x.Equals((object)y));
        }

        [TestMethod]
        public void TestToString()
        {
            var result = new Padding(1, 2, 3, 4).ToString();
            var expected = "Padding(Left = 1, Top = 2, Right = 3, Bottom = 4)";
            Assert.AreEqual(expected, result);
        }
    }
}