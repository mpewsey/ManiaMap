using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectableIdEntry
    {
        [TestMethod]
        public void TestToString()
        {
            var obj = new CollectableIdEntry(1, 2);
            Assert.IsTrue(obj.ToString().StartsWith("CollectableIdEntry("));
        }
    }
}