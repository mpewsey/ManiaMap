using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestUid
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 3);
            Assert.IsTrue(x == y);
            Assert.IsFalse(x.Equals(null));
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestInitializers()
        {
            var x = new Uid(1);
            Assert.AreEqual(1, x.Value1);
            Assert.AreEqual(0, x.Value2);
            Assert.AreEqual(0, x.Value3);

            var y = new Uid(1, 2);
            Assert.AreEqual(1, y.Value1);
            Assert.AreEqual(2, y.Value2);
            Assert.AreEqual(0, y.Value3);

            var z = new Uid(1, 2, 3);
            Assert.AreEqual(1, z.Value1);
            Assert.AreEqual(2, z.Value2);
            Assert.AreEqual(3, z.Value3);
        }

        [TestMethod]
        public void TestComparison()
        {
            var x1 = new Uid(1);
            var y1 = new Uid(2);
            Assert.AreEqual(-1, x1.CompareTo(y1));

            var x2 = new Uid(1);
            var y2 = new Uid(1);
            Assert.AreEqual(0, x2.CompareTo(y2));

            var x3 = new Uid(1, 1);
            var y3 = new Uid(1, 2);
            Assert.AreEqual(-1, x3.CompareTo(y3));
        }
    }
}