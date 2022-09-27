using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoorEntry
    {
        [TestMethod]
        public void TestToString()
        {
            var obj = new DoorEntry(DoorDirection.West, Door.TwoWay);
            Assert.IsTrue(obj.ToString().StartsWith("DoorEntry("));
        }
    }
}