using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectableEntry
    {
        [TestMethod]
        public void TestToString()
        {
            var obj = new CollectableEntry(1, "Group1");
            Assert.IsTrue(obj.ToString().StartsWith("CollectableEntry("));
        }
    }
}