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
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            Assert.IsTrue(x != y);
        }
    }
}